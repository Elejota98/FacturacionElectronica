using Controlador;
using FirebirdSql.Data.FirebirdClient;
using Modelo;
using QRCoder;
using Servicios;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;

namespace FacturacionElectronicaFrm
{
    public partial class FrmNotaCredito : Form
    {
        public FrmNotaCredito()
        {
            InitializeComponent();
            ListarEstacionamientos();
        }
        #region Definiciones
        private string _ConnectionStringFirebird
        {
            get
            {
                string sSerial = ConfigurationManager.AppSettings["ConexionLocal"];
                if (string.IsNullOrEmpty(sSerial))
                {
                    return "User ID=SYSDBA;Password=masterkey;Database=C://magister/datos/magisterz.mgt;DataSource=localhost;Charset=NONE;";
                }
                else
                {
                    return sSerial;
                }
            }
        }
        public string idModulo = string.Empty;
        #endregion


        #region Funciones 

        public void ListarEstacionamientos()
        {
                cboEstacionamientos.DataSource = NotaCreditoController.ListarEstacionamientos();
            cboEstacionamientos.DisplayMember = "Nombre";
                cboEstacionamientos.ValueMember = "IdEstacionamiento";
        }

        public void ListarPagosInterfaz()
        {
            DataTable tablaDatosInterfaz;
            tablaDatosInterfaz = NotaCreditoController.ListarPagosInterfaz(Convert.ToInt32(cboEstacionamientos.SelectedValue), dtmFecha.Text);
            if (tablaDatosInterfaz.Rows.Count > 0)
            {
                dvgListadoInterfaz.DataSource = NotaCreditoController.ListarPagosInterfaz(Convert.ToInt32(cboEstacionamientos.SelectedValue), dtmFecha.Text);

                dvgListadoInterfaz.Columns[0].Visible = true;
                lblRtaFacturas.Visible = true;
                lblRtaFacturas.Text = "Resultados facturas electrónicas de " + cboEstacionamientos.Text + " de la fecha " + dtmFecha.Text + "";
                MensajeAListBox("Se encontraron, " + tablaDatosInterfaz.Rows.Count + " Registros");
               

            }
            else
            {
                MensajeAListBox("Sin datos para la interfaz, en caso de algún error comunicarse con Tecnología");

                MessageBox.Show("No existe información o la factura de la fecha seleccionada ya se le hizo nota crédito", "Parquearse Tecnología");
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
                         idModulo = fila.Cells[6].Value.ToString();
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

                            if (NotaCreditoController.InsertarItemsContable(tablaDatos, consecutivo, Convert.ToInt32(cboEstacionamientos.SelectedValue), numeroFactura, idModulo))
                            {
                                #region AnularFactura
                                //string rtaPos = string.Empty;
                                //DataTable tablaPagos;
                                //tablaPagos = NotaCreditoController.ListarPagosAnular(Convert.ToInt32(cboEstacionamientos.SelectedValue), Convert.ToDateTime(dtmFecha.Text), Convert.ToInt32(numeroFactura));

                                //if (tablaPagos.Rows.Count > 0)
                                //{

                                //    foreach (DataRow lstPagos in tablaPagos.Rows)
                                //    {
                                //        int idPago = Convert.ToInt32(lstPagos["IdPago"]);

                                //        rtaPos = NotaCreditoController.AnularFacturaPOS(idPago);
                                //        if (rtaPos.Equals("OK"))
                                //        {
                                //            MensajeAListBox("Se anuló la factura Pos con Id " + idPago + "");
                                //        }

                                //    }

                                //    MensajeAListBox("Finaliza escritura con estacionamiento = " + Convert.ToInt32(cboEstacionamientos.SelectedValue));
                                //}
                                #endregion

                            MensajeAListBox("Se generó la nota de credito de manera correcta");

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

        private void button2_Click_1(object sender, EventArgs e)
        {
            ListarItemsContable();
        }

        private void button4_Click_1(object sender, EventArgs e)
        {
       
        }

        private void button3_Click_2(object sender, EventArgs e)
        {
            DataTable tabla;
            tabla = FacturacionElectronicaController.ListarClientesNuevos();
            if (tabla.Rows.Count > 0)
            {
                foreach (DataRow row in tabla.Rows)
                {
                    string valor = Convert.ToString(row["Identificacion"]);
                    string correo = Convert.ToString(row["Email"]);
                    string rut = Convert.ToString(row["rut"]);

                    DataTable tablaPorDoc = FacturacionElectronicaController.ListarClientesNuevosPorDoc(Convert.ToInt32(valor));

                    if (tablaPorDoc.Rows.Count > 0)
                    {
                        ActualizaEstadoCliente(Convert.ToInt32(valor));
                        MensajeAListBox("El cliente con identificación " + valor + " ya se encuentra activo");
                    }
                }
            }
        }

        public bool ActualizaEstadoCliente(int identificacion)
        {
            string rta = "";
            bool ok = false;
            rta = FacturacionElectronicaController.ActualizaEstadoCliente(identificacion);
            if (rta.Equals("OK"))
            {
                ok = true;
            }
            else
            {
                ok = false;
            }
            return ok;
        }
    }
}
