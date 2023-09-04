using Controlador;
using Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.TaskbarClock;

namespace FacturacionElectronicaServicio
{
    public partial class SincronizacionFacturacionElectronica : Form
    {
        #region Definiciones
        Cliente cliente = new Cliente();
        Pagos pagos = new Pagos();
        Cotizaciones cotizaciones = new Cotizaciones();
        CotizacionEncabezado cotizacionEncabezado = new CotizacionEncabezado();
        public int numeroFacturaAnterior = 1;
        public int cotNum = 1;
        public int itemCounter = 1;
        public string sRutaCarpeta = ConfigurationManager.AppSettings["RutaArchvoClientes"];


        public string PrefijoFacturasElectronicas()
        {
            string Prefijo = (ConfigurationManager.AppSettings["PrefijoFE"]);
            return Prefijo;
        }

        #endregion
        public SincronizacionFacturacionElectronica()
        {
            InitializeComponent();
        }

        #region Funciones

                public void GuardarRegistrosEnExcel(string rutaArchivo, DataTable tabla)
        {
            using (StreamWriter writer = new StreamWriter(rutaArchivo))
            {
                foreach (DataRow fila in tabla.Rows)
                {
                    foreach (DataColumn columna in tabla.Columns)
                    {
                        writer.WriteLine(columna.ColumnName + ": " + fila[columna]);
                    }

                    writer.WriteLine(); // Agregar una línea en blanco entre cada fila
                }
            }
        }

        //CLIENTES
        public bool ListarClientes()
        {
            bool ok = false;
            DataTable tabla = new DataTable();


            tabla = FacturacionElectronicaController.ListarClientes();
            if (tabla.Rows.Count > 0)
            {
                foreach (DataRow row in tabla.Rows)
                {
                    
                    cliente.Identificacion= Convert.ToInt32(row["Identificacion"].ToString());
                    cliente.RazonSocial = row["RazonSocial"].ToString();
                    cliente.Direccion = row["Direccion"].ToString();
                    cliente.Telefono = row["Telefono"].ToString();
                    cliente.Email = row["Email"].ToString();
                    cliente.Nombre = row["Nombre"].ToString();
                    cliente.Fecha = Convert.ToDateTime(row["Fecha"]);
                    cliente.FechaSeparada = cliente.Fecha.ToString("dd.MM.yyyy HH.mm");
                    cliente.Estado = Convert.ToBoolean(row["Estado"]);
                    cliente.Rut = Convert.ToString(row["Rut"].ToString());
                    cliente.Vendedor = ListarDocumentoVendedor();
                    cliente.Empresa = 1;

                    if (VerificarClienteExiste(cliente))
                    {
                        if (cliente.Rut != string.Empty)
                        {
                            byte[] pdfData = (byte[])tabla.Rows[0]["Rut"];
                            if (pdfData != null && pdfData.Length > 0)
                            {
                                string nombreArchivoPDF = cliente.Identificacion + "-" + cliente.Email + ".pdf";
                                string rutaArchivoPDF = Path.Combine(sRutaCarpeta, nombreArchivoPDF);
                                File.WriteAllBytes(rutaArchivoPDF, pdfData);
                                string nombreArchivoTexto = cliente.Identificacion + ".txt";
                                string rutaArchivoTexto = Path.Combine(sRutaCarpeta, nombreArchivoTexto);
                                GuardarRegistrosEnExcel(rutaArchivoTexto, tabla);
                                //ActualizaEstadoCliente(Convert.ToInt32(valor));
                            }
                            else
                            {
                                string nombreArchivoTexto = cliente.Identificacion + ".txt";
                                string rutaArchivoTexto = Path.Combine(sRutaCarpeta, nombreArchivoTexto);
                                GuardarRegistrosEnExcel(rutaArchivoTexto, tabla);
                            }
                        }

                        if (ActualizarEstadoCliente(cliente))
                        {

                            ok = true;
                        }
                        else
                        {
                            ok = false;
                        }
                    }
                    else
                    {
                        if (cliente.Rut != string.Empty)
                        {
                            byte[] pdfData = (byte[])tabla.Rows[0]["Rut"];
                            if (pdfData != null && pdfData.Length > 0)
                            {
                                string nombreArchivoPDF = cliente.Identificacion + "-" + cliente.Email + ".pdf";
                                string rutaArchivoPDF = Path.Combine(sRutaCarpeta, nombreArchivoPDF);
                                File.WriteAllBytes(rutaArchivoPDF, pdfData);
                                string nombreArchivoTexto = cliente.Identificacion + ".txt";
                                string rutaArchivoTexto = Path.Combine(sRutaCarpeta, nombreArchivoTexto);
                                GuardarRegistrosEnExcel(rutaArchivoTexto, tabla);
                                //ActualizaEstadoCliente(Convert.ToInt32(valor));
                            }
                            else
                            {
                                string nombreArchivoTexto = cliente.Identificacion + ".txt";
                                string rutaArchivoTexto = Path.Combine(sRutaCarpeta, nombreArchivoTexto);
                                GuardarRegistrosEnExcel(rutaArchivoTexto, tabla);
                            }
                        }


                        if (InsertarClientesInterfazMagister(cliente))
                        {
                            if (ActualizarEstadoCliente(cliente))
                            {

                                ok = true;
                            }
                            else
                            {
                                ok = false;
                            }
                        }
                        else
                        {
                            ok = false;
                        }
                    }
                }

            }
            return ok;
        }
        public int ListarDocumentoVendedor()
        {
            DataTable tabla = new DataTable();
            int documento = 0;
            try
            {
                tabla = FacturacionElectronicaController.ListarDocumentoVendedor();
                foreach (DataRow rtaTabla in tabla.Rows)
                {
                     documento = Convert.ToInt32(rtaTabla["VEN_IDENTIFICACION"]);

                }
            }
            catch (Exception ex )
            {

                throw ex ;
            }
            return documento;
        }
        public bool InsertarClientesInterfazMagister(Cliente cliente)
        {
            bool ok = false;
            string rta = "";
            rta = FacturacionElectronicaController.InsertarClienteInterfaz(cliente);
            if (!rta.Equals("ERROR"))
            {
                GenerarArchivoPlano(rta);
                ok = true;
            }
            else
            {
                GenerarArchivoPlano(rta);
                ok = false;
            }
            return ok;

        }
        public bool ActualizarEstadoCliente(Cliente cliente)
        {
            bool ok = false;
            string rta = "";
            rta = FacturacionElectronicaController.ActualizaEstadoCliente(cliente);
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
        public bool VerificarClienteExiste(Cliente cliente)
        {
            DataTable tabla;
            bool ok =false;
            tabla = FacturacionElectronicaController.ValidarExisteCliente(cliente);
            if (tabla.Rows.Count > 0)
            {
                ok = true;
            }
            else
            {
                return ok;
            }
            return ok;

        }
        public void InicioProceso()
        {
            try
            {
                if (ListarClientes())
                {
                    MensajeAListBox("Clientes registrados en la interfaz");
                }
                else
                {
                    if (ListarPagos())
                    {
                        MensajeAListBox("Pagos registrados en la interfaz");
                    }
                }
            }
            catch (Exception ex )
            {

                MensajeAListBox(ex.ToString());
            }
        }


        //PAGOS 

        public bool ListarPagos()
        {
            DataTable tabla;
            bool ok = false;
            tabla = FacturacionElectronicaController.ListarPagos();
            if (tabla.Rows.Count > 0)
            {
                Thread.Sleep(10000);
                tabla = FacturacionElectronicaController.ListarPagos();
                if (tabla.Rows.Count > 0)
                {
                    cotizaciones.Cot_Item = 1;

                    foreach (DataRow lstPagos in tabla.Rows)
                    {
                        pagos.Id = Convert.ToInt32(lstPagos["Id"]);
                        pagos.Empresa = Convert.ToInt32(lstPagos["Empresa"]);
                        pagos.FechaPago = Convert.ToDateTime(lstPagos["FechaPago"]);
                        pagos.Identificacion = Convert.ToInt32(lstPagos["Identificacion"]);
                        pagos.CodigoSucursal = Convert.ToInt32(lstPagos["CodigoSucursal"]);
                        pagos.Prefijo = lstPagos["Prefijo"].ToString();
                        pagos.NumeroFactura = Convert.ToInt32(lstPagos["NumeroFactura"]);
                        pagos.Total = Convert.ToInt32(lstPagos["Total"]);
                        pagos.IdEstacionamiento = Convert.ToInt32(lstPagos["IdEstacionamiento"]);
                        pagos.IdTipoPago = Convert.ToInt32(lstPagos["IdTipoPago"]);
                        pagos.Vendedor = Convert.ToInt32(lstPagos["Vendedor"]);

                        //COTIZACIONES

                        cotizaciones.Cot_Centro_Costo = ListarCentroDeCosto(pagos);
                        cotizaciones.Cot_Numero = ListarCotizaciones();
                        cotizaciones.Cot_Cantidad = 1;
                        cotizaciones.Cot_Valor_Unitario = pagos.Total;
                        cotizaciones.Cot_Documento = PrefijoFacturasElectronicas();
                        cotizaciones.Cot_Tipo_Item = 2;
                        cotizaciones.Cot_Empresa = pagos.Empresa;

                        if (numeroFacturaAnterior != pagos.NumeroFactura)
                        {
                            cotNum = cotizaciones.Cot_Numero + 1;
                        }
                        else
                        {
                            cotNum = cotizaciones.Cot_Numero;

                        }
                        cotizaciones.Cot_Numero=cotNum;

                        if (pagos.IdTipoPago == 1)
                        {
                            cotizaciones.Cot_Referencia = "05";
                        }
                        else if (pagos.IdTipoPago == 2)
                        {
                            cotizaciones.Cot_Referencia = "06";
                        }
                        else if (pagos.IdTipoPago == 3)
                        {
                            cotizaciones.Cot_Referencia = "30";
                        }
                        else if (pagos.IdTipoPago == 4)
                        {
                            cotizaciones.Cot_Referencia = "31";
                        }
                        else if (pagos.IdTipoPago == 5)
                        {
                            cotizaciones.Cot_Referencia = "41";
                        }
                        else if (pagos.IdTipoPago == 6)
                        {
                            cotizaciones.Cot_Referencia = "98";
                        }


                        numeroFacturaAnterior = pagos.NumeroFactura;


                        if (InsertarPagosInterfaz(cotizaciones))
                        {
                            cotizaciones.Cot_Item++;
                            MensajeAListBox("Sincronizacion Pago OK número de factura " + pagos.NumeroFactura + "");
                            ActualizaEstadoPagos(pagos);
                            MensajeAListBox("Actualizó Pago OK número de factura " + pagos.NumeroFactura + " Con Id "+pagos.Id+"");
                            ok = true;

                        }
                        else
                        {
                            MensajeAListBox("Hubo un error en el momento de insertar los pagos a la interfaz");
                            ok =false;
                        }



                    }

                    //Cotizaciones Encabezado

                    DateTime fechaHoy = DateTime.Now;
                    string fechaStr = fechaHoy.ToString("yyyy-MM-dd");

                    DateTime fechaNum = DateTime.ParseExact(fechaStr, "yyyy-MM-dd", null);
                    cotizacionEncabezado.Coe_Fecha = (int)(fechaHoy - new DateTime(1899, 12, 30)).TotalDays;
                    cotizacionEncabezado.Coe_Observaciones = "Reemplazo factura POS " + pagos.Prefijo + " - " + pagos.NumeroFactura + "";
                    cotizacionEncabezado.Coe_Empresa = cotizaciones.Cot_Empresa;
                    cotizacionEncabezado.Coe_Documento = PrefijoFacturasElectronicas();
                    cotizacionEncabezado.Coe_Numero = cotizaciones.Cot_Numero;
                    cotizacionEncabezado.Coe_Cliente = pagos.Identificacion;
                    cotizacionEncabezado.Coe_Cliente_Sucursal = 1;
                    cotizacionEncabezado.Coe_Sincronizado = 0;
                    cotizacionEncabezado.Coe_Forma_Pago = 1;
                    cotizacionEncabezado.Coe_Vendedor = ListarDocumentoVendedor();

                    if (InsertaPagosCotizacionEncabezado(cotizacionEncabezado))
                    {
                        ok=true;
                        MensajeAListBox("Sincronizó Tabla cotización encabezado OK");
                    }
                    else
                    {
                        ok=false;
                    }
                }


            }
            return ok;


        }

        public bool InsertarPagosInterfaz(Cotizaciones cotizaciones)
        {
            try
            {
                bool ok = false;
                string rta = "";
                rta = FacturacionElectronicaController.InsetarPagosInterfaz(cotizaciones);
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
            catch (Exception ex )
            {

                throw ex ;
            }

        }

        public bool ActualizaEstadoPagos(Pagos pagos)
        {
            try
            {
                bool ok = false;
                string rta = "";
                rta = FacturacionElectronicaController.ActualizaEstadoPagos(pagos);
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
            catch (Exception ex )
            {

                throw  ex;
            }
        }

        public string ListarCentroDeCosto(Pagos pagos)
        {
            string centroCosto = string.Empty;
            DataTable tabla = new DataTable();


            tabla = FacturacionElectronicaController.ListarCentroCosto(pagos);
            if (tabla.Rows.Count > 0)
            {

                centroCosto = tabla.Rows[0]["CentroCosto"].ToString();
            }
            return centroCosto;
        }
        public int ListarCotizaciones()
        {
            int numeroCotizacion = 0;
            DataTable tabla = new DataTable();
            tabla = FacturacionElectronicaController.ListarCotizaciones();
            if (tabla.Rows.Count > 0)
            {
                foreach (DataRow lstCotizaciones in tabla.Rows)
                {
                     numeroCotizacion = Convert.ToInt32(lstCotizaciones["COT_NUMERO"]);

                }
            }
            return numeroCotizacion;
        }

        public bool InsertaPagosCotizacionEncabezado(CotizacionEncabezado cotizacionEncabezado)
        {
            try
            {
                bool ok = false;
                string rta = "";
                rta = FacturacionElectronicaController.InsertaPagosCotizacionEncabezado(cotizacionEncabezado);
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
            catch (Exception ex)
            {

                throw ex;
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

        private void Inicio_Tick(object sender, EventArgs e)
        {
            InicioProceso();
        }
        private void SincronizacionFacturacionElectronica_Load(object sender, EventArgs e)
        {
            MensajeAListBox("Inicia el proceso a la espera de información a sincronizar");
            Inicio.Start();
            this.WindowState = FormWindowState.Minimized;

        }

        private void notifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
        }

        private void restaurarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Show();
        }

        private void cerrarToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void SincronizacionFacturacionElectronica_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon1.Icon = SystemIcons.Application;
                notifyIcon1.BalloonTipText = "Consultado Información en segundo plano";
                notifyIcon1.ShowBalloonTip(1000);
            }
        }
    }
}
