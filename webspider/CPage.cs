using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using System.Collections;

namespace webspider
{
    class CPage
    {
        protected String m_szURL;
        protected String m_szID;
        protected Uri m_uURL;
        protected String m_szContent = "";
        protected ArrayList m_saLinks;
        protected ArrayList m_saSegmentos;
        protected bool IsColombian;
        protected CountryLookup m_pContryLookup;

        public CPage(string id, string url, CountryLookup lookup)
        {
            m_pContryLookup = lookup;
            m_szID = id;
            m_saLinks = new ArrayList();
            try
            {
                SetURL(url);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        void SetURL(string url)
        {
            m_saSegmentos = new ArrayList();
            m_szURL = url;
            try
            {
                m_uURL = new Uri(url);
            } catch(Exception ex) {
                throw ex;
            }
            IsColombian = false;
            IsColombian = IsValid(url);
            for (int i = 0; i < m_uURL.Segments.Length; i++)
            {
                if (m_uURL.Segments[i][m_uURL.Segments[i].Length - 1] != '/')
                    break;
                m_saSegmentos.Add(m_uURL.Segments[i]);
            }
        }

        public bool IsSameDomain(string url)
        {
            try
            {
                Uri temp = new Uri(url);
                return temp.DnsSafeHost == m_uURL.DnsSafeHost;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error url: " + url + ": " + ex.ToString());
            }
            return false;
        }

        public bool Store(CConector pConector )
        {   
            CQuery query = new CQuery("url");
            query["cont"] = m_szContent;
            query["actualizacion"] = "SYSDATE()";
            if (m_szID.Length == 0)
                m_szID = pConector.QueryString("SELECT id FROM url WHERE url='" + m_szURL + "'");

            if (m_szID.Length == 0)
            {
                query["url"] = m_szURL;
                query["descargando"]= query["creacion"] = "SYSDATE()";
                int resultado = pConector.QueryMod(query.GetInsert());
                if(resultado==-1)
                    return false;
                m_szID= resultado.ToString();
            }
            else
            {
                if (pConector.QueryMod(query.GetUpdate("id='" + m_szID + "'")) == -1)
                    return false;
            }

            // almacenamos los links
            for (int i = 0; i < m_saLinks.Count; i++)
            {
                if(IsSameDomain(m_saLinks[i].ToString())) // referencias internas
                    pConector.QueryMod("UPDATE url SET cint=cint+1 WHERE url='" + m_saLinks[i].ToString() + "'");
                else pConector.QueryMod("UPDATE url SET cint=cext+1 WHERE url='" + m_saLinks[i].ToString() + "'"); //referencias externas

                CQuery links = new CQuery("url");
                links["url"] = m_saLinks[i].ToString();
                links["creacion"] = "SYSDATE()";

                pConector.QueryMod(links.GetInsert().Replace("INSERT INTO ", "INSERT IGNORE INTO "));
            }
            return true;
        }

        public bool IsValid(string url)
        {
            if (IsColombian)
                return true;
            try
            {
                Uri temp = new Uri(url); // verificamos que el dominio sea colombiano
                string co = temp.DnsSafeHost.Substring(temp.DnsSafeHost.Length - 3);
                if (co == ".co")
                {
                    return true;
                }
                IPAddress[] ips;
                ips = Dns.GetHostAddresses(temp.DnsSafeHost);
                foreach (IPAddress ip in ips)
                {
                    if (m_pContryLookup.lookupCountryCode(ip.ToString()) == "CO")
                        return true;
                }
            }
            catch (UriFormatException ex)
            {
                Console.WriteLine("Error url: " + url + ": " + ex.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error url: " + url + ": " + ex.ToString());
            }

            return false;
        }

        public string GetURL(string url)
        {
            string final = m_uURL.Scheme + "://" + m_uURL.Authority;
            if (url.Length == 0)
                return final;
            if (url[0] == '/')
                return final + url;
            int segmentos = m_saSegmentos.Count;
            if (url.Length > 2)
                if (url.Substring(0, 2) == "./")
                    url = url.Remove(0, 2);
            
            if (url.Length == 0)
                return final;
            
            if (url.Length > 3)
            {
                while (url.Substring(0, 3) == "../")
                {
                    segmentos--;
                    url = url.Remove(0, 3);
                }
            }
            
            for (int i = 0; i < segmentos; i++)
            {
                final+=m_saSegmentos[i].ToString();
            }
            return final + url;
        }

        public bool AddURL(string url)
        {
            url = url.Trim();
            if (url.Length==0 || url[0] == '#' || url.IndexOf("mailto:")==0) // hace referencia a la misma pagina o es un anclaje
            {
                return false;
            }
            int pos = url.IndexOf("http://"); // es una URL completa
            if (pos != 0)
            {
                url = GetURL(url); // esta en el mismo servidor
            }

            if (url == m_szURL)
                return false;
            for (int i = 0; i < m_saLinks.Count; i++)
            {
                if (url.ToLower() == m_saLinks[i].ToString().ToLower())
                    return false;
            }
            if (IsValid(url))
            {
                m_saLinks.Add(url);
                return true;
            }
            return false;
        }

        public int ParseLinks()
        {
            int pos = 0;
            string contenido = m_szContent.ToLower();
            while ((pos = contenido.IndexOf("<a", pos)) != -1) // buscamos los links
            {
                int final = contenido.IndexOf(">", pos); // buscamos el final del link
                if (final != -1)
                {
                    string link = m_szContent.Substring(pos, final - pos + 1);
                    int href= link.ToLower().IndexOf("href");
                    if (href != -1)
                    {
                        string urlreal="";
                        int i;
                        for (i = href + 4; i < link.Length; i++)
                        {
                            if (link[i] == '\"' || link[i] == '\'')
                            {
                                i++;
                                break;
                            }
                        }
                        if (i < link.Length) // no se llego al final
                        {
                            for (; i < link.Length; i++)
                            {
                                if (link[i] == '\"' || link[i] == '\'')
                                {
                                    break;
                                }
                                urlreal += link[i];
                            }
                            AddURL(urlreal);
                        }
                    }
                }
                pos += 2;
            }
            return m_saLinks.Count;
        }

        public bool Donwload() {

            try
            {
                // Creates an HttpWebRequest with the specified URL. 
                HttpWebRequest myHttpWebRequest = (HttpWebRequest)WebRequest.Create(m_szURL);
                // Sends the HttpWebRequest and waits for the response.			
                HttpWebResponse myHttpWebResponse = (HttpWebResponse)myHttpWebRequest.GetResponse();
                // Gets the stream associated with the response.
                Stream receiveStream = myHttpWebResponse.GetResponseStream();
                Encoding encode = System.Text.Encoding.GetEncoding("utf-8");
                // Pipes the stream to a higher level stream reader with the required encoding format. 
                StreamReader readStream = new StreamReader(receiveStream, encode);
                
                Char[] read = new Char[1024];
                // Reads 256 characters at a time.    
                int count = readStream.Read(read, 0, 1024);
                while (count > 0)
                {
                    // Dumps the 256 characters on a string and displays the string to the console.
                    String str = new String(read, 0, count);
                    if (m_szContent.Length == 0)
                    {
                        if(!str.ToLower().Trim().Contains("<html")) // solo descargamos paginas webs
                            break;
                    }
                    m_szContent += str;
                    count = readStream.Read(read, 0, 1024);
                }
                SetURL(myHttpWebResponse.ResponseUri.ToString()); // actualizamos la direcion por algun redirect
                Console.WriteLine("");
                // Releases the resources of the response.
                myHttpWebResponse.Close();
                // Releases the resources of the Stream.
                readStream.Close();
            }
            catch (System.Net.WebException ex)
            {
                Console.WriteLine(ex.ToString());
                return false;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
                return false;
            }
            return true;
        }
    }
}
