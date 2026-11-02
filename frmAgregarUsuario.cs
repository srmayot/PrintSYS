using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using PrintSYS.Datos;
using Microsoft.Data.SqlClient;

namespace PrintSYS
{
    public partial class frmAgregarUsuario : Form
    {
        public frmAgregarUsuario()
        {
            InitializeComponent();
        }

        private void btnAgregar_Click(object sender, EventArgs e)
        {
            string nombre = txtNombre.Text.Trim();

            MessageBox.Show("Nombre recibido: " + nombre);

            if (string.IsNullOrEmpty(nombre))
            {
                MessageBox.Show("Ingrese un nombre");
                return;
            }

            ConexionBD conexionBD = new ConexionBD();

            using (SqlConnection conexion = conexionBD.ConnectBD())
            {
                {
                    string consulta = @"
            INSERT INTO TB_ReclamadoPor (ReclamadoPor, Activo)
            VALUES (@ReclamadoPor, 1)";

                    using (SqlCommand comando = new SqlCommand(consulta, conexion))
                    {
                        comando.Parameters.AddWithValue("@ReclamadoPor", nombre);

                        comando.ExecuteNonQuery();
                    }
                }

                MessageBox.Show("Usuario agregado correctamente.");

                txtNombre.Clear();
                txtNombre.Focus();
            }
        }
    }
}
