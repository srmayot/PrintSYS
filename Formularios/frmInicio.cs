using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace PrintSYS.Formularios
{
    public partial class frmInicio : Form
    {
        public frmInicio()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Normal;
        }

        private void frmInicio_Load(object sender, EventArgs e)
        {

        }

        private void btnEmpezar_Click(object sender, EventArgs e)
        {
            /*frmGestion form2 = new frmGestion();
            this.Hide();    //Oculta el form1
            form2.ShowDialog();
            this.Close();   //Cierra el form1 luego de abrir el 2*/


            //Cierra el form sin animación
            frmGestion form2 = new frmGestion();
            form2.FormClosed += (s, args) => this.Close();
            form2.Show();
            this.Hide();
        }
    }
}
