using System;
using System.Configuration;
using Microsoft.Data.SqlClient;

namespace PrintSYS.Datos
{
    public class ConexionBD
    {
        private static ConexionBD conn = null;

        public ConexionBD() { }

        public SqlConnection ConnectBD()
        {
            SqlConnection conexionSQL = new SqlConnection();

            string cadena = ConfigurationManager
                .ConnectionStrings["PrintSYS.cn"].ConnectionString;

            conexionSQL.ConnectionString = cadena;
            conexionSQL.Open();

            return conexionSQL;
        }

        public static ConexionBD creaInstancia()
        {
            if (conn == null)
                conn = new ConexionBD();
            return conn;
        }
    }
}