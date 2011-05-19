using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using MySql.Data.MySqlClient;

namespace webspider
{
    class Program
    {
        static void Main(string[] args)
        {
            string url = "";
            string id = "";
            if (args.Length > 0)
            {
                url= args[0];
            }
            CountryLookup cl = new CountryLookup("GeoIP.dat");

            while (true)
            {
                CConector conector = new CConector();
                if (url.Length == 0)
                {
                    MySqlDataReader reader = conector.Query("SELECT id,url FROM url WHERE descargando IS NULL LIMIT 1");
                    if (reader.Read())
                    {
                        id = reader["id"].ToString();
                        url = reader["url"].ToString();
                    }
                    else
                    {
                        Console.Error.WriteLine("No se encontraron URLs.");
                        break;
                    }
                    conector.QueryMod("UPDATE url SET descargando=SYSDATE() WHERE id='" + id.ToString() + "'");
                }
                try
                {
                    CPage page = new CPage(id, url, cl);
                    Console.WriteLine(url);
                    page.Donwload();
                    Console.WriteLine(page.ParseLinks() + " Links");
                    page.Store(conector);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                }
                url = "";
            }
        }
    }
}
