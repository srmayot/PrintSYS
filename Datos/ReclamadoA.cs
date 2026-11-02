using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrintSYS.Datos
{
    internal class ReclamadoA
    {
        public DataTable listarReclamadoA()
        {
            SqlDataReader resultado;
            DataTable tabla = new DataTable();
            SqlConnection sqlcon = new SqlConnection();

            try
            {
                sqlcon = ConexionBD.creaInstancia().ConnectBD();
                SqlCommand comando = new SqlCommand("SP_LISTAR_RECLAMADO_A", sqlcon);
                comando.CommandType = CommandType.StoredProcedure;

                //sqlcon.Open();

                resultado = comando.ExecuteReader();
                tabla.Load(resultado);

                return tabla;

            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en cargar la Lista " + ex.Message, "PrintSYS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }
            finally
            {
                if (sqlcon.State == ConnectionState.Open)
                {
                    sqlcon.Close();
                }
            }
        }
    }
}
