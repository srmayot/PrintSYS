using ClosedXML.Excel;
using PrintSYS.Datos;
using System;
using System.Data;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace PrintSYS.Formularios
{
    public partial class frmGestion : Form
    {
        int idImpresora = 0;
        public frmGestion()
        {
            InitializeComponent();
            this.WindowState = FormWindowState.Normal;
        }

        #region "METODOS"
        //Cargar Reclamos en el Dgv
        private void cargarReclamos(string cBusqueda)
        {
            Impresora reclamos = new Impresora();
            dgvLista.DataSource = reclamos.listarReclamos(cBusqueda);

            // OCULTAR IDS en el dgv
            dgvLista.Columns["Id_Suc"].Visible = false;
            dgvLista.Columns["Id_Cargo"].Visible = false;
            dgvLista.Columns["Id_ReclamadoA"].Visible = false;
            dgvLista.Columns["Id_ReclamadoPor"].Visible = false;
            dgvLista.Columns["Id_MedioRec"].Visible = false;
            dgvLista.Columns["Id_Reserva"].Visible = false;
            dgvLista.Columns["Id_Estado"].Visible = false;
        }

        //Formato de la lista del Dgv
        private void formatoListaReclamos()
        {
            dgvLista.Columns["ID"].Width = 30;
            dgvLista.Columns["Modelo"].Width = 90;
            dgvLista.Columns["NroSerial"].Width = 80;

            dgvLista.Columns["Sucursal"].Width = 120;
            dgvLista.Columns["DetalleProblema"].Width = 300;

            dgvLista.Columns["IP"].Width = 80;
            dgvLista.Columns["NroTicket"].Width = 100;

            dgvLista.Columns["DiasAtrasados"].Width = 60;
        }

        //Cargar Modelos CMB
        private void cargarModelo()
        {
            ModeloImp dato = new ModeloImp();
            cmbModelo.DataSource = dato.listarModelos();
            cmbModelo.ValueMember = "IdModelo";
            cmbModelo.DisplayMember = "Modelo";
            cmbModelo.SelectedIndex = -1;
        }

        //Cargar Sucursal CMB
        private void cargarSucursal()
        {
            Sucursal dato = new Sucursal();
            cmbSucursal.DataSource = dato.listarSucursal();
            cmbSucursal.ValueMember = "IdSuc";
            cmbSucursal.DisplayMember = "Sucursal";
            cmbSucursal.SelectedIndex = -1;
        }
        //Cargar Cargos CMB
        private void cargarCargos()
        {
            Cargos dato = new Cargos();
            cmbCargoContc.DataSource = dato.listarCargos();
            cmbCargoContc.ValueMember = "IdCargo";
            cmbCargoContc.DisplayMember = "Cargo";
            cmbCargoContc.SelectedIndex = -1;
            ;
        }
        //Cargar Reclamado A ComboBox
        public void cargarReclamadoA()
        {
            ReclamadoA dato = new ReclamadoA();
            cmbReclamadoA.DataSource = dato.listarReclamadoA();
            cmbReclamadoA.ValueMember = "IdReclamadoA";
            cmbReclamadoA.DisplayMember = "ReclamadoA";
            cmbReclamadoA.SelectedIndex = -1;
        }
        //Cargar MedioReclamo CMB
        public void cargarMedioReclamo()
        {
            MedioRec medio = new MedioRec();
            cmbMedioRec.DataSource = medio.listarMedioReclamo();
            cmbMedioRec.ValueMember = "IdMedio";
            cmbMedioRec.DisplayMember = "MedioReclamo";
            cmbMedioRec.SelectedIndex = -1;
        }
        //Cargar Reclamado/por CMB
        public void cargarReclamadoPor()
        {
            ReclamadoPor dato = new ReclamadoPor();
            cmbReclamdoPor.DataSource = dato.listarReclamadoPor();
            cmbReclamdoPor.ValueMember = "IdReclamadoPor";
            cmbReclamdoPor.DisplayMember = "ReclamadoPor";
            cmbReclamdoPor.SelectedIndex = -1;
        }


        #endregion

        private void frmGestion_Load(object sender, EventArgs e)
        {

            cargarReclamos("%");
            formatoListaReclamos();

            cargarModelo();
            cargarSucursal();
            cargarCargos();
            cargarReclamadoA();
            cargarMedioReclamo();
            cargarReclamadoPor();
            configurarOrdenTab();

            dtpFechaInicio.CustomFormat = "dd/MM/yyyy";
            dtpFechaInicio.Format = DateTimePickerFormat.Custom;

            // --- CONFIGURACIÓN CORRECTA DE LA FECHA DE FIN ---
            dtpFechaFin.ShowCheckBox = true; // 1. Obligatorio para permitir que quede "vacío" (desmarcado)
            dtpFechaFin.Format = DateTimePickerFormat.Custom;
            dtpFechaFin.CustomFormat = "dd/MM/yyyy";
            dtpFechaFin.Checked = false; // 2. Arranca desmarcado por defecto (reclamo abierto)
                                         // ------------------------------------------------

            DataTable dt = new DataTable();
            dt.Columns.Add("Id");
            dt.Columns.Add("Texto");

            dt.Rows.Add(1, "SI");
            dt.Rows.Add(2, "NO");

            cmbImpReserva.DataSource = dt;
            cmbImpReserva.DisplayMember = "Texto";
            cmbImpReserva.ValueMember = "Id";
            cmbImpReserva.SelectedIndex = -1;

            Estado obj = new Estado();
            cmbEstadoReclamo.DataSource = obj.listarEstado();
            cmbEstadoReclamo.ValueMember = "IdEstado";
            cmbEstadoReclamo.DisplayMember = "Estado";
            cmbEstadoReclamo.SelectedIndex = -1;

        }

        private void btnSalir_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        //Link Znuny de ejemplo
        private void linkOtrs_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            string url = "https://ticket.empresa.com.py/znuny/index.pl?Action=AgentTicketQueue;QueueID=36;View=;Filter=Unlocked";

            try
            {
                System.Diagnostics.Process.Start(new ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true // Importante para que funcione en .NET Core y versiones recientes
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("No se pudo abrir el enlace: " + ex.Message);
            }
        }

        private void btnGuardar_Click(object sender, EventArgs e)
        {
            // =========================
            // VALIDACIONES BÁSICAS
            // =========================
            string errores = "";

            // OBLIGATORIOS PRINCIPALES
            if (cmbSucursal.SelectedIndex == -1)
                errores += "- Sucursal\n";

            if (cmbModelo.SelectedIndex == -1)
                errores += "- Modelo\n";

            if (string.IsNullOrWhiteSpace(txtSerial.Text))
                errores += "- Nro Serial\n";

            if (string.IsNullOrWhiteSpace(txtDetalles.Text))
                errores += "- Detalle del problema\n";

            if (cmbEstadoReclamo.SelectedIndex == -1)
                errores += "- Estado\n";

            // FOREIGN KEYS OBLIGATORIOS
            if (cmbReclamadoA.SelectedIndex == -1)
                errores += "- Reclamado A\n";

            if (cmbReclamdoPor.SelectedIndex == -1)
                errores += "- Reclamado Por\n";

            if (cmbMedioRec.SelectedIndex == -1)
                errores += "- Medio de Reclamo\n";

            if (cmbCargoContc.SelectedIndex == -1)
                errores += "- Cargo\n";

            if (cmbImpReserva.SelectedIndex == -1)
                errores += "- Reserva\n";

            // DATOS DE CONTACTO
            if (string.IsNullOrWhiteSpace(txtNombrContc.Text))
                errores += "- Nombre de contacto\n";

            if (string.IsNullOrWhiteSpace(txtTelContc.Text))
                errores += "- Teléfono\n";

            // --- VALIDACIONES DE CONTENIDO TÉCNICO Y FECHAS ---

            // 1. Validar formato de IP
            if (string.IsNullOrWhiteSpace(txtIP.Text))
            {
                errores += "- IP (Campo vacío)\n";
            }
            else if (!System.Net.IPAddress.TryParse(txtIP.Text.Trim(), out _))
            {
                errores += "- IP (Formato inválido)\n";
            }

            // 2. Validar N° de Ticket (exactamente 8 dígitos numéricos)
            string ticketValidacion = txtNroTicket.Text.Trim();
            if (string.IsNullOrWhiteSpace(ticketValidacion))
            {
                errores += "- N° Ticket (Campo vacío)\n";
            }
            else if (ticketValidacion.Length != 8 || !ticketValidacion.All(char.IsDigit))
            {
                errores += "- N° Ticket (Debe tener exactamente 8 dígitos numéricos)\n";
            }

            // 3. Validar Fechas (Fecha de fin no puede ser menor a la de inicio si está activada)
            if (dtpFechaFin.Format != DateTimePickerFormat.Custom || dtpFechaFin.Checked)
            {
                if (dtpFechaFin.Value.Date < dtpFechaInicio.Value.Date)
                {
                    errores += "- Fechas (La fecha de fin no puede ser anterior a la de inicio)\n";
                }
            }

            if (errores != "")
            {
                MessageBox.Show("Complete los siguientes campos:\n\n" + errores,
                                "Validación",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Warning);
                return;
            }

            // =========================
            // PROCESO GUARDAR
            // =========================
            try
            {
                Impresora imp = new Impresora();

                int reserva = Convert.ToInt32(cmbImpReserva.SelectedValue);
                int estado = Convert.ToInt32(cmbEstadoReclamo.SelectedValue);

                DateTime? fechaFin = null;

                if (dtpFechaFin.Format != DateTimePickerFormat.Custom || dtpFechaFin.Checked)
                {
                    fechaFin = dtpFechaFin.Value;
                }

                bool guardado;

                // INSERTAR
                if (idImpresora == 0)
                {
                    guardado = imp.insertarReclamo(
                        Convert.ToInt32(cmbModelo.SelectedValue),
                        txtSerial.Text.Trim(),
                        Convert.ToInt32(cmbSucursal.SelectedValue),
                        txtDetalles.Text.Trim(),
                        txtIP.Text.Trim(),
                        txtNroTicket.Text.Trim(),
                        txtNombrContc.Text.Trim(),
                        txtTelContc.Text.Trim(),
                        Convert.ToInt32(cmbCargoContc.SelectedValue),
                        Convert.ToInt32(cmbReclamadoA.SelectedValue),
                        Convert.ToInt32(cmbMedioRec.SelectedValue),
                        Convert.ToInt32(cmbReclamdoPor.SelectedValue),
                        dtpFechaInicio.Value,
                        fechaFin,
                        reserva,
                        estado,
                        txtObs.Text.Trim()
                    );
                }
                // ACTUALIZAR
                else
                {
                    guardado = imp.actualizarReclamo(
                        idImpresora,
                        Convert.ToInt32(cmbModelo.SelectedValue),
                        txtSerial.Text.Trim(),
                        Convert.ToInt32(cmbSucursal.SelectedValue),
                        txtDetalles.Text.Trim(),
                        txtIP.Text.Trim(),
                        txtNroTicket.Text.Trim(),
                        txtNombrContc.Text.Trim(),
                        txtTelContc.Text.Trim(),
                        Convert.ToInt32(cmbCargoContc.SelectedValue),
                        Convert.ToInt32(cmbReclamadoA.SelectedValue),
                        Convert.ToInt32(cmbMedioRec.SelectedValue),
                        Convert.ToInt32(cmbReclamdoPor.SelectedValue),
                        dtpFechaInicio.Value,
                        fechaFin,
                        reserva,
                        estado,
                        txtObs.Text.Trim()
                    );
                }

                // =========================
                // RESULTADO
                // =========================
                if (guardado)
                {
                    MessageBox.Show("Operación realizada correctamente.",
                                    "PrintSYS",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Information);

                    cargarReclamos("%");
                    formatoListaReclamos();
                    btnCancelar_Click(sender, e);
                    idImpresora = 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al guardar: " + ex.Message,
                                "PrintSYS",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
        }

        private void btnCancelar_Click(object sender, EventArgs e)
        {
            cmbModelo.SelectedIndex = -1;
            txtSerial.Clear();
            cmbSucursal.SelectedIndex = -1;
            txtDetalles.Clear();
            txtIP.Clear();
            txtNroTicket.Clear();
            txtNombrContc.Clear();
            txtTelContc.Clear();
            cmbCargoContc.SelectedIndex = -1;
            cmbReclamadoA.SelectedIndex = -1;
            cmbMedioRec.SelectedIndex = -1;
            cmbReclamdoPor.SelectedIndex = -1;
            dtpFechaInicio.Value = DateTime.Today;
            dtpFechaFin.Value = DateTime.Today;
            cmbImpReserva.SelectedIndex = -1;
            cmbEstadoReclamo.SelectedIndex = -1;
            txtObs.Clear();
            txtDiasAtraso.Clear();

            dtpFechaInicio.Value = DateTime.Today;

            dtpFechaFin.Value = DateTime.Today;
            dtpFechaFin.Checked = false;
            dtpFechaFin.Enabled = false;
            dtpFechaFin.Format = DateTimePickerFormat.Custom;
            dtpFechaFin.CustomFormat = " ";
        }

        //BOTON BUSCAR
        private void btnBuscar_Click(object sender, EventArgs e)
        {
            cargarReclamos(txtBuscar.Text);
        }

        //Cuadro de texto buscar --- igual función que btnBuscar
        private void txtBuscar_TextChanged(object sender, EventArgs e)
        {
            cargarReclamos(txtBuscar.Text);
        }

        private void btnReporte_Click(object sender, EventArgs e)
        {
            try
            {
                if (dgvLista.Rows.Count == 0)
                {
                    MessageBox.Show("No hay datos para exportar.");
                    return;
                }

                SaveFileDialog sfd = new SaveFileDialog();
                sfd.Filter = "Excel (*.xlsx)|*.xlsx";
                sfd.FileName = "Reclamos.xlsx";

                if (sfd.ShowDialog() == DialogResult.OK)
                {
                    using (var wb = new XLWorkbook())
                    {
                        var ws = wb.Worksheets.Add("Reclamos");

                        int colIndex = 0;
                        for (int i = 0; i < dgvLista.Columns.Count; i++)
                        {
                            if (dgvLista.Columns[i].Visible)
                            {
                                var cell = ws.Cell(1, colIndex + 1);
                                cell.Value = dgvLista.Columns[i].HeaderText;
                                cell.Style.Font.Bold = true;
                                colIndex++;
                            }
                        }

                        for (int i = 0; i < dgvLista.Rows.Count; i++)
                        {
                            colIndex = 0;

                            for (int j = 0; j < dgvLista.Columns.Count; j++)
                            {
                                if (dgvLista.Columns[j].Visible)
                                {
                                    object valor = dgvLista.Rows[i].Cells[j].Value;

                                    if (valor != null && valor != DBNull.Value)
                                        ws.Cell(i + 2, colIndex + 1).Value = XLCellValue.FromObject(valor);

                                    colIndex++;
                                }
                            }
                        }

                        ws.Columns().AdjustToContents();

                        wb.SaveAs(sfd.FileName);
                    }

                    MessageBox.Show("Excel exportado correctamente.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al exportar: " + ex.Message);
            }
        }

        private void btnEditar_Click(object sender, EventArgs e)
        {

            if (dgvLista.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un registro.", "PrintSYS", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            idImpresora = Convert.ToInt32(dgvLista.CurrentRow.Cells["ID"].Value);

            txtSerial.Text = dgvLista.CurrentRow.Cells["NroSerial"].Value?.ToString();
            txtDetalles.Text = dgvLista.CurrentRow.Cells["DetalleProblema"].Value?.ToString();
            txtIP.Text = dgvLista.CurrentRow.Cells["IP"].Value?.ToString();
            txtNroTicket.Text = dgvLista.CurrentRow.Cells["NroTicket"].Value?.ToString();
            txtNombrContc.Text = dgvLista.CurrentRow.Cells["Nombre"].Value?.ToString();
            txtTelContc.Text = dgvLista.CurrentRow.Cells["Telefono"].Value?.ToString();
            txtObs.Text = dgvLista.CurrentRow.Cells["Observacion"].Value?.ToString();


            var fechaInicio = dgvLista.CurrentRow.Cells["Fecha_Inicio_Rec"].Value;
            if (fechaInicio != null && fechaInicio != DBNull.Value)
            {
                dtpFechaInicio.Format = DateTimePickerFormat.Custom;
                dtpFechaInicio.CustomFormat = "dd/MM/yyyy";
                dtpFechaInicio.Value = Convert.ToDateTime(fechaInicio);
            }

            /*var fechaFin = dgvLista.CurrentRow.Cells["Fecha_Fin_Rec"].Value;
            dtpFechaFin.Enabled = true;

            if (fechaFin != null && fechaFin != DBNull.Value && fechaFin.ToString() != "")
            {
                dtpFechaFin.Format = DateTimePickerFormat.Custom;
                dtpFechaFin.CustomFormat = "dd/MM/yyyy";
                dtpFechaFin.Checked = true;
                dtpFechaFin.Value = Convert.ToDateTime(fechaFin);
            }
            else
            {
                dtpFechaFin.Format = DateTimePickerFormat.Custom;
                dtpFechaFin.CustomFormat = " ";
                dtpFechaFin.Checked = false;
            }*/

            var fechaFin = dgvLista.CurrentRow.Cells["Fecha_Fin_Rec"].Value;
            dtpFechaFin.Enabled = true;

            if (fechaFin != null && fechaFin != DBNull.Value && fechaFin.ToString() != "")
            {
                dtpFechaFin.Format = DateTimePickerFormat.Custom;
                dtpFechaFin.CustomFormat = "dd/MM/yyyy";
                dtpFechaFin.Checked = true;
                dtpFechaFin.Value = Convert.ToDateTime(fechaFin);
            }
            else
            {
                dtpFechaFin.Format = DateTimePickerFormat.Custom;
                dtpFechaFin.CustomFormat = "dd/MM/yyyy";
                dtpFechaFin.Value = DateTime.Today;
                dtpFechaFin.Checked = true;
            }

            // COMBOS POR ID
            cmbSucursal.SelectedValue = dgvLista.CurrentRow.Cells["Id_Suc"].Value;
            cmbCargoContc.SelectedValue = dgvLista.CurrentRow.Cells["Id_Cargo"].Value;
            cmbReclamadoA.SelectedValue = dgvLista.CurrentRow.Cells["Id_ReclamadoA"].Value;
            cmbMedioRec.SelectedValue = dgvLista.CurrentRow.Cells["Id_MedioRec"].Value;
            cmbReclamdoPor.SelectedValue = dgvLista.CurrentRow.Cells["Id_ReclamadoPor"].Value;
            cmbImpReserva.SelectedValue = dgvLista.CurrentRow.Cells["Id_Reserva"].Value;
            cmbEstadoReclamo.SelectedValue = dgvLista.CurrentRow.Cells["Id_Estado"].Value;

            // MODELO POR TEXTO
            cmbModelo.Text = dgvLista.CurrentRow.Cells["Modelo"].Value?.ToString();

            MessageBox.Show("Modo edición activado. Modificá los campos y presioná Guardar.", "PrintSYS", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void btnEliminar_Click(object sender, EventArgs e)
        {

            if (dgvLista.CurrentRow == null)
            {
                MessageBox.Show("Seleccione un registro.");
                return;
            }

            DialogResult r = MessageBox.Show(
                "¿Seguro que desea eliminar este reclamo?",
                "Confirmar",
                MessageBoxButtons.YesNo,
                MessageBoxIcon.Warning
            );

            if (r == DialogResult.No)
                return;

            try
            {
                int id = Convert.ToInt32(dgvLista.CurrentRow.Cells["ID"].Value);

                Impresora imp = new Impresora();
                bool eliminado = imp.eliminarReclamo(id);

                if (eliminado)
                {
                    MessageBox.Show("Reclamo eliminado correctamente.");

                    cargarReclamos("%");
                    formatoListaReclamos();
                }
                else
                {
                    MessageBox.Show("No se pudo eliminar.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.Message);
            }
        }

        private void btnActualizar_Click(object sender, EventArgs e)
        {
            cargarReclamos("%");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            idImpresora = 0;

            txtSerial.Clear();
            txtDetalles.Clear();
            txtIP.Clear();
            txtNroTicket.Clear();
            txtNombrContc.Clear();
            txtTelContc.Clear();
            txtObs.Clear();
            txtDiasAtraso.Clear();

            cmbModelo.SelectedIndex = -1;
            cmbSucursal.SelectedIndex = -1;
            cmbEstadoReclamo.SelectedIndex = -1;
            cmbImpReserva.SelectedIndex = -1;
            cmbMedioRec.SelectedIndex = -1;
            cmbCargoContc.SelectedIndex = -1;
            cmbReclamadoA.SelectedIndex = -1;
            cmbReclamdoPor.SelectedIndex = -1;

            dtpFechaInicio.Value = DateTime.Now;

            dtpFechaFin.Checked = false;
            dtpFechaFin.Format = DateTimePickerFormat.Custom;
            dtpFechaFin.CustomFormat = " ";

            MessageBox.Show("Formulario listo para nuevo registro.");
        }
        private Control[] ordenTab;

        private void configurarOrdenTab()
        {
            ordenTab = new Control[]
            {
        cmbModelo,
        txtSerial,
        cmbSucursal,
        txtDetalles,
        txtIP,
        txtNroTicket,
        txtNombrContc,
        txtTelContc,
        cmbCargoContc,
        cmbReclamadoA,
        cmbMedioRec,
        cmbReclamdoPor,
        dtpFechaInicio,
        dtpFechaFin,
        cmbImpReserva,
        cmbEstadoReclamo,
        txtObs,
        txtDiasAtraso
            };
        }

        protected override bool ProcessCmdKey(ref Message msg, Keys keyData)
        {
            if (ordenTab == null) return base.ProcessCmdKey(ref msg, keyData);

            if (keyData == Keys.Tab || keyData == (Keys.Tab | Keys.Shift))
            {

                int actual = -1;
                for (int i = 0; i < ordenTab.Length; i++)
                {
                    if (ordenTab[i].Focused ||
                        (ordenTab[i] is ComboBox && ((ComboBox)ordenTab[i]).DroppedDown))
                    {
                        actual = i;
                        break;
                    }
                }

                if (actual >= 0)
                {
                    int siguiente;
                    if (keyData == (Keys.Tab | Keys.Shift))
                        siguiente = (actual - 1 + ordenTab.Length) % ordenTab.Length;
                    else
                        siguiente = (actual + 1) % ordenTab.Length;

                    ordenTab[siguiente].Focus();
                    return true;
                }
            }

            return base.ProcessCmdKey(ref msg, keyData);
        }

        private void cmbModelo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void agregarUsuarioToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            frmAgregarUsuario formulario = new frmAgregarUsuario();
            formulario.ShowDialog();
        }
    }
}
