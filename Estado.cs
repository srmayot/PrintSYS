using System.Data;
using Microsoft.Data.SqlClient;
using PrintSYS.Datos;

namespace PrintSYS.Datos
{
    public class Estado
    {
        public DataTable listarEstado()
        {
            DataTable tabla = new DataTable();

            using (SqlConnection conn = ConexionBD.creaInstancia().ConnectBD())
            {
                SqlCommand cmd = new SqlCommand("SELECT IdEstado, Estado FROM TB_ESTADO", conn);
                SqlDataAdapter da = new SqlDataAdapter(cmd);
                da.Fill(tabla);
            }

            return tabla;
        }
    }
}
