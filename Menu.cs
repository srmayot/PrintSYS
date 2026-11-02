using PrintSYS.Formularios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PrintSYS
{
    internal static class Menu
    {
        /// <summary>
        /// Punto de entrada principal para la aplicación.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            // Captura errores inesperados
            Application.ThreadException += (s, e) =>
                MessageBox.Show("Error inesperado: " + e.Exception.Message,
                    "PrintSYS", MessageBoxButtons.OK, MessageBoxIcon.Error);

            AppDomain.CurrentDomain.UnhandledException += (s, e) =>
                MessageBox.Show("Error crítico: " + e.ExceptionObject.ToString(),
                    "PrintSYS", MessageBoxButtons.OK, MessageBoxIcon.Error);

            Application.Run(new frmInicio());
        }
    }
}
