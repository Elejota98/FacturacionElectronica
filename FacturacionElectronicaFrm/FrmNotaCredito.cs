using Controlador;
using FirebirdSql.Data.FirebirdClient;
using Modelo;
using QRCoder;
using Servicios;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FacturacionElectronicaFrm
{
    public partial class FrmNotaCredito : Form
    {
        public FrmNotaCredito()
        {
            InitializeComponent();
            ListarEstacionamientos();
        }


        #region Funciones 

        public void ListarEstacionamientos()
        {
                cboEstacionamientos.DataSource = NotaCreditoController.ListarEstacionamientos();
            cboEstacionamientos.DisplayMember = "Nombre";
                cboEstacionamientos.ValueMember = "IdEstacionamiento";
        }

        public void ListarPagosInterfaz()
        {

            dvgListadoInterfaz.DataSource = NotaCreditoController.ListarPagosInterfaz(Convert.ToInt32(cboEstacionamientos.SelectedValue), dtmFecha.Text);
            if (dvgListadoInterfaz.Rows.Count > 0)
            {
                dvgListadoInterfaz.DataSource = NotaCreditoController.ListarPagosInterfaz(Convert.ToInt32(cboEstacionamientos.SelectedValue), dtmFecha.Text);

                dvgListadoInterfaz.Columns[0].Visible = true;
                lblRtaFacturas.Visible = true;
                lblRtaFacturas.Text = "Resultados facturas electrónicas de " + cboEstacionamientos.Text + " de la fecha " + dtmFecha.Text + "";
               

            }
        }

        public void ListarInterfaz()
        {
            try
            {
                //int numeroFactura = 0;
                //dvgListadoInterfaz.DataSource = NotaCreditoController.ListarPagosInterfaz(Convert.ToInt32(cboEstacionamientos.SelectedValue), dtmFecha.Text);
                //foreach (DataRow lstTabla in dvgListadoInterfaz.Rows)
                //{
                //     numeroFactura = Convert.ToInt32(lstTabla["NumeroFactura"]);

                //}

                List<string> numerosFacturaSeleccionados = new List<string>();
                foreach (DataGridViewRow fila in dvgListadoInterfaz.Rows)
                {
                    DataGridViewCheckBoxCell checkbox = fila.Cells[0] as DataGridViewCheckBoxCell;
                    if (Convert.ToBoolean(checkbox.Value))
                    {
                        string numeroFactura = fila.Cells[5].Value.ToString();
                        numerosFacturaSeleccionados.Add(numeroFactura);
                    }
                }
                foreach (string numeroFactura in numerosFacturaSeleccionados)
                {
                    DataTable tabla;
                    DataTable tablaEmpresa;
                    int idc_Empresa = 1;
                    string doc_Empresa = "NCRP";
                    string rta = "";
                    //tabla= NotaCreditoController.ListarInterfaz(Convert.ToInt32(cboEstacionamientos.SelectedValue), numeroFactura, dtmFecha.Text);
                    //tabla = NotaCreditoController.ListarInterfaz(Convert.ToInt32(cboEstacionamientos.SelectedValue), numeroFactura, dtmFecha.Text);

                    //List<ItemsContable> itemsContables = new List<ItemsContable>();
                    //foreach (DataRow item in tabla.Rows)
                    //{
                    //    ItemsContable items = new ItemsContable();
                    //    items.IDC_DOCUMENTO = Convert.ToInt32(ListarIdcEmpresa());
                    //    items.IDC_NUMERO = item["IDC_NUMERO"].ToString();

                    //LISTO DATOS EMPRESA
                    

                    //FIN DATOS EMPRESA 

                    //VALIDO SI EL REGISTRO YA SE SUBIO 
                    DateTime fechaNew;
                    fechaNew = Convert.ToDateTime(dtmFecha.Text);
                    

                    tabla = NotaCreditoController.VerificarSiExisteElRegistro(Convert.ToDateTime(fechaNew.ToString("dd-MM-yyyy")), idc_Empresa, doc_Empresa, numeroFactura);
                    if (tabla.Rows.Count <= 0)
                    {
                        //GENERAR PROCEDIMIENTO ALMACENADO 
                        DataTable tablaDatos;
                        tablaDatos = NotaCreditoController.GenerarDatosASubir(Convert.ToInt32(cboEstacionamientos.SelectedValue), Convert.ToDateTime(dtmFecha.Text), Convert.ToInt32(numeroFactura));
                        if (tablaDatos != null && tablaDatos.Rows.Count > 0)
                        {
                            //INSERTTAR INFORMACION EN LA INTERFAZ
                            int consecutivo = 1;

                            if (NotaCreditoController.InsertarItemsContable(tablaDatos, consecutivo, Convert.ToInt32(cboEstacionamientos.SelectedValue), numeroFactura))
                            {
                                #region AnularFactura
                                string rtaPos = string.Empty;
                                DataTable tablaPagos;
                                tablaPagos = NotaCreditoController.ListarPagosAnular(Convert.ToInt32(cboEstacionamientos.SelectedValue), Convert.ToDateTime(dtmFecha.Text), Convert.ToInt32(numeroFactura));

                                if (tablaPagos.Rows.Count > 0)
                                {

                                    foreach (DataRow lstPagos in tablaPagos.Rows)
                                    {
                                        int idPago = Convert.ToInt32(lstPagos["IdPago"]);

                                        rtaPos = NotaCreditoController.AnularFacturaPOS(idPago);
                                        if (rtaPos.Equals("OK"))
                                        {
                                            MensajeAListBox("Se anuló la factura Pos con Id " + idPago + "");
                                        }

                                    }
                                    #endregion
                                    MensajeAListBox("Finaliza escritura con estacionamiento = " + Convert.ToInt32(cboEstacionamientos.SelectedValue));
                                }
                                }
                            else
                            {
                                MensajeAListBox("Falla escritura con estacionamiento = " + Convert.ToInt32(cboEstacionamientos.SelectedValue));
                            }

                        }

                    }
                    else
                    {
                        MensajeAListBox("El registro con número "+numeroFactura+" ya se encuentra en la  interfaz");
                    }

                }
            }
            catch (Exception ex )
            {

                throw ex ;
            }



            //string mensaje = ObtenerMensajeDeTabla(tabla);
            //MensajeAListBox(mensaje);
            //GenerarArchivoPlano(mensaje);


        }
        private (int docEmpresa, int idcEmpresa) ListarIdcEmpresa()
        {
            DataTable tabla;
            int doc_Empresa = 0;
            int idc_Empresa = 0;
            tabla = NotaCreditoController.ListarDocumentoEmpresa(Convert.ToInt32(cboEstacionamientos.SelectedValue));
            foreach (DataRow lst in tabla.Rows)
            {
                doc_Empresa = Convert.ToInt32( lst["DocumentoEmpresa"].ToString());
                idc_Empresa = Convert.ToInt32(lst["Idc_Empresa"].ToString());

            }
            return (doc_Empresa, idc_Empresa);

            }
        private string ObtenerMensajeDeTabla(DataTable tabla)
        {
            StringBuilder sb = new StringBuilder();
            foreach (DataRow row in tabla.Rows)
            {
                foreach (DataColumn col in tabla.Columns)
                {
                    sb.Append(row[col.ColumnName].ToString());
                    sb.Append(" ");
                }
                sb.AppendLine();
            }
            return sb.ToString();
        }

        public void GenerarArchivoPlano(string texto)
        {


            // Obtener la fecha actual para el nombre de archivo
            DateTime fechaActual = DateTime.Now;
            string rutaFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Registro NC" + fechaActual.Day.ToString() + "-" + fechaActual.Month.ToString() + "-" + fechaActual.Year.ToString());
            string nombre = "NotaCredito";
            string path = rutaFolder + "/" + nombre + @".txt";

            if (!Directory.Exists(rutaFolder))
            {
                Directory.CreateDirectory(rutaFolder);
            }

            using (StreamWriter sw = new StreamWriter(path, true, Encoding.UTF8))
            {
                sw.WriteLine(texto);

                MensajeAListBox("Se generó el documento .txt OK");
            }

        }


        public void ObtenerResultadoInterfaz()
        {
          
        }

        //INTERFAZ 

        public void ListarItemsContable()
        {
            dvgListadoInterfaz.DataSource = NotaCreditoController.ListarItemsContable();
        }

        public void EliminarIntemsContable()
        {
            FbConnection fbCon = new FbConnection();
            try
            {
                fbCon = RepositorioConexion.getInstancia().CrearConexionLocal();
                string cadena = ("DELETE FROM ITEMSDOCCONTABLE");
                FbCommand comando = new FbCommand(cadena,fbCon);
                fbCon.Open();
                comando.ExecuteNonQuery();
            }
            catch (Exception ex )
            {

                throw ex ;
            }
            finally
            {
                if (fbCon.State == ConnectionState.Open) fbCon.Close();
            }
        }

        #endregion


        #region Mensajes 
        private void MensajeAListBox(string mensaje)
        {
            lbEventos.Items.Add(DateTime.Now.ToString("dd/MM/yy HH:mm:ss") + " -> " + mensaje);
            this.lbEventos.SelectedIndex = this.lbEventos.Items.Count - 1;
            //TraceHandler.WriteLine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Logs\Log"), "MENSAJE: " + mensaje, TipoLog.TRAZA);
        }
        #endregion

        private void btnConsultar_Click(object sender, EventArgs e)
        {
            //EliminarIntemsContable();
            ListarPagosInterfaz();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ListarInterfaz();
        }

        private void button2_Click(object sender, EventArgs e)
        {

            ListarItemsContable();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dvgListadoInterfaz.DataSource = NotaCreditoController.ListarDocContable();
        }

        private void button4_Click(object sender, EventArgs e)
        {

        }

        private void chkSelect_CheckedChanged(object sender, EventArgs e)
        {
            int columnIndex = 0; 
            int columna2= 1;

            foreach (DataGridViewRow row in dvgListadoInterfaz.Rows)
            {
                DataGridViewCheckBoxCell checkBoxCell = row.Cells[columnIndex] as DataGridViewCheckBoxCell;

                if (row.Cells[columna2].Value != null)
                {
                    checkBoxCell.Value = true; 

                }
            }
        }
    }
}
