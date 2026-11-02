using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Windows.Forms;

namespace PrintSYS.Datos
{
    public class Impresora
    {
        public DataTable listarReclamos(string cBusqueda)
        {
            DataTable tabla = new DataTable();

            try
            {
                using (SqlConnection sqlcon = ConexionBD.creaInstancia().ConnectBD())
                using (SqlCommand comando = new SqlCommand("SP_LISTAR_RECLAMOS", sqlcon))
                {
                    comando.CommandType = CommandType.StoredProcedure;
                    comando.Parameters.Add("@cBusqueda", SqlDbType.VarChar).Value = cBusqueda;

                    using (SqlDataReader resultado = comando.ExecuteReader())
                    {
                        tabla.Load(resultado);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error en cargar la Lista " + ex.Message, "PrintSYS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                throw;
            }

            return tabla;
        }

        public bool insertarReclamo(
            int idModelo, string serial, int idSucursal,
            string detalle, string ip, string ticket, string nombre, string telefono,
            int idCargo, int idReclamadoA, int idMedio, int idReclamadoPor,
            DateTime fechaInicio, DateTime? fechaFin,
            int impReserva,
            int estado, string obs) // <-- QUITAMOS string diasAtraso de aquí
        {
            try
            {
                using (SqlConnection sqlcon = ConexionBD.creaInstancia().ConnectBD())
                using (SqlCommand cmd = new SqlCommand("SP_INSERTAR_RECLAMO", sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdModelo", idModelo);
                    cmd.Parameters.AddWithValue("@Serial", serial);
                    cmd.Parameters.AddWithValue("@IdSucursal", idSucursal);
                    cmd.Parameters.AddWithValue("@Detalle", detalle);
                    cmd.Parameters.AddWithValue("@IP", ip);
                    cmd.Parameters.AddWithValue("@Ticket", ticket);
                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    cmd.Parameters.AddWithValue("@Telefono", telefono);
                    cmd.Parameters.AddWithValue("@IdCargo", idCargo);
                    cmd.Parameters.AddWithValue("@IdReclamadoA", idReclamadoA);
                    cmd.Parameters.AddWithValue("@IdMedio", idMedio);
                    cmd.Parameters.AddWithValue("@IdReclamadoPor", idReclamadoPor);
                    cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);

                    cmd.Parameters.AddWithValue("@FechaFin",
                        (object)fechaFin ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@ImpReserva", impReserva);
                    cmd.Parameters.AddWithValue("@Estado", estado);
                    cmd.Parameters.AddWithValue("@OBS", obs);

                    // QUITAMOS esta línea por completo:
                    // cmd.Parameters.AddWithValue("@DiasAtraso", diasAtraso);

                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al insertar: " + ex.Message, "PrintSYS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool actualizarReclamo(
    int idImpresora,
    int idModelo,
    string serial,
    int idSucursal,
    string detalle,
    string ip,
    string ticket,
    string nombre,
    string telefono,
    int idCargo,
    int idReclamadoA,
    int idMedio,
    int idReclamadoPor,
    DateTime fechaInicio,
    DateTime? fechaFin,
    int reserva,
    int estado,
    string obs) // <-- QUITAMOS string diasAtraso de aquí
        {
            try
            {
                using (SqlConnection sqlcon = ConexionBD.creaInstancia().ConnectBD())
                using (SqlCommand cmd = new SqlCommand("SP_UPDATE_RECLAMO", sqlcon))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@IdImpresora", idImpresora);
                    cmd.Parameters.AddWithValue("@IdModelo", idModelo);
                    cmd.Parameters.AddWithValue("@Serial", serial);
                    cmd.Parameters.AddWithValue("@IdSucursal", idSucursal);
                    cmd.Parameters.AddWithValue("@DetalleProblema", detalle);
                    cmd.Parameters.AddWithValue("@IP", ip);
                    cmd.Parameters.AddWithValue("@NroTicket", ticket);
                    cmd.Parameters.AddWithValue("@Nombre", nombre);
                    cmd.Parameters.AddWithValue("@Telefono", telefono);
                    cmd.Parameters.AddWithValue("@IdCargo", idCargo);
                    cmd.Parameters.AddWithValue("@IdReclamadoA", idReclamadoA);
                    cmd.Parameters.AddWithValue("@IdMedioRec", idMedio);
                    cmd.Parameters.AddWithValue("@IdReclamadoPor", idReclamadoPor);
                    cmd.Parameters.AddWithValue("@FechaInicio", fechaInicio);

                    cmd.Parameters.AddWithValue("@FechaFin",
                        (object)fechaFin ?? DBNull.Value);

                    cmd.Parameters.AddWithValue("@IdReserva", reserva);
                    cmd.Parameters.AddWithValue("@IdEstado", estado);
                    cmd.Parameters.AddWithValue("@Observacion", obs);

                    // QUITAMOS esta línea por completo:
                    // cmd.Parameters.AddWithValue("@DiasAtrasados", diasAtraso);

                    cmd.ExecuteNonQuery();
                    return true;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al actualizar: " + ex.Message);
                return false;
            }
        }

        public bool eliminarReclamo(int id)
        {
            try
            {
                using (SqlConnection sqlcon = ConexionBD.creaInstancia().ConnectBD())
                using (SqlCommand cmd = new SqlCommand("UPDATE TB_IMPRESORA SET activo_imp = 0 WHERE IdImpresora = @Id", sqlcon))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    int filas = cmd.ExecuteNonQuery();
                    return filas > 0;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error al eliminar: " + ex.Message, "PrintSYS", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

        }
    }
} 
