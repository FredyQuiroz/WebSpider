using System;
using System.Configuration;
using MySql.Data.MySqlClient;

public class CConector
{
    protected string m_szLastError;
    protected string m_szQuery;

    protected MySqlConnection m_pSQLConector;
    protected MySqlDataReader m_pSQLReader;

    public CConector()
    {
        // cargamos los valores por defect
        m_szQuery = "";
        m_szLastError = "";
        m_pSQLConector = null;
        m_pSQLReader = null;
    }

    ~CConector()
    {
        try
        {
            if (m_pSQLReader != null)
            {
                m_pSQLReader.Close();
                m_pSQLReader.Dispose();
            }
            if (m_pSQLConector != null)
            {
                m_pSQLConector.Clone();
                m_pSQLConector.Dispose();
            }
        }
        catch (MySqlException ex)
        {
            Console.Error.WriteLine(ex.ToString());
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.ToString());
        }
    }

    public MySqlConnection GetConexion()
    {
        return m_pSQLConector;
    }
    
    protected bool IsConect()
    {
        if (m_pSQLConector == null)  // verificamos que la conexion exista
        {
            // solo por guardar compatibilidad con la version anterior 'coneccion'
            string conn = Environment.GetEnvironmentVariable("WEBSPIDER_CONNECTION_STRING");
            try
            {
                m_pSQLConector = new MySqlConnection(conn);
                m_pSQLConector.Open();
            }
            catch (MySqlException ex)
            {
                m_szLastError = ex.Message;
                Console.Error.WriteLine(m_szLastError);
                return false;
            }
            return true;
        }
        return true;
    }

    public MySqlDataAdapter QueryAdaptador(string query)
    {
        return new MySqlDataAdapter(query, Environment.GetEnvironmentVariable("WEBSPIDER_CONNECTION_STRING"));
    }

    public int QueryMod(string query)
    {
        return QueryMod(query, false);
    }

    public int QueryMod(string query, bool identity)
    {
        m_szQuery = query;
        if (query.IndexOf(" WHERE") == 0)
            return 0;
        int result = 0;
        if (!IsConect())
            return -1;
        if (m_pSQLReader != null)
        {
            m_pSQLReader.Close();
            m_pSQLReader.Dispose();
            m_pSQLReader = null;
        }
        try
        {
            MySqlCommand command = m_pSQLConector.CreateCommand();
            command.CommandText = query;
            result = command.ExecuteNonQuery();
            command.Dispose();
        }
        catch (MySqlException ex)
        {
            result = -1;
            m_szLastError = ex.Message;
            Console.Error.WriteLine(m_szLastError);
            Console.Error.WriteLine(query);
        }
        if (identity && result != -1)
        {
            result = -1;
            if (Query("SELECT LAST_INSERT_ID()") != null)
            {
                if (m_pSQLReader.Read())
                {
                    result = Convert.ToInt32(m_pSQLReader[0].ToString());
                }
                m_pSQLReader.Close();
                m_pSQLReader.Dispose();
                m_pSQLReader = null;
            }
        }
        return result;
    }

    public string QueryString(string query)
    {
        MySqlDataReader data = Query(query);
        string retorno = "";
        if (data != null && data.Read())
        {
            retorno = data[0].ToString();
        }
        if(data==null)
            data.Close();
        return retorno;
    }

    public MySqlDataReader Query(string query)
    {
        return Query(query, false);
    }

    public MySqlDataReader Query(string query, bool reset)
    {
        if (!IsConect())
        {
            return null;
        }
        if (query == null || query.Length == 0) {
            m_szLastError = "";
            return null;
        }
        m_szQuery = query;
        if (m_pSQLReader != null)
        {
            m_pSQLReader.Close();
            m_pSQLReader.Dispose();
            m_pSQLReader = null;
        }
        try
        {
            MySqlCommand command = m_pSQLConector.CreateCommand();
            command.CommandText = query;
	        command.CommandTimeout=0;
            m_pSQLReader = command.ExecuteReader();
            command.Dispose();
        }
        catch (MySqlException ex)
        {
            m_pSQLReader = null;
            m_szLastError = ex.Message;
            Console.Error.WriteLine(m_szLastError);
            Console.Error.WriteLine(query);
        }
        return m_pSQLReader;
    }

    public int GetColCount()
    {
        if (m_pSQLReader == null)
            return 0;
        return m_pSQLReader.FieldCount;
    }

    public string GetLastError()
    {
        if (m_szLastError.Length == 0)
            return "";
        return m_szLastError + "<br>" + m_szQuery;
    }

    public static string GetCondicion(string campo, string dato)
    {
        return GetCondicion(campo, dato, "=");
    }

    public static string GetCondicion(string campo, string dato, string comparador)
    {
        return " WHERE " + campo + " " + comparador + " '" + dato + "'";
    }
    public static string GetDate(object fecha)
    {
        if (fecha == null || fecha.ToString().Length == 0)
            return "";
        DateTime tiempo = (DateTime)fecha;
        string mes= tiempo.Month<10 ? ("0" + tiempo.Month) : tiempo.Month.ToString();
        string dia = tiempo.Day < 10 ? ("0" + tiempo.Day) : tiempo.Day.ToString();
        return tiempo.Year + "-" + mes + "-" + dia;
    }
    public static string GetDateFormat(string fecha) {
        if(fecha.Length==0 || fecha.Length<10) 
            return "";
        return fecha.Substring(0, 10);
    }
}
