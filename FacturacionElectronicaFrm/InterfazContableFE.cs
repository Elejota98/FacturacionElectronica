using Controlador;
using FirebirdSql.Data.FirebirdClient;
using Modelo;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Media;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace FacturacionElectronicaFrm
{


    public partial class Form1 : Form
    {
        #region Variables
        public string textoCliente = string.Empty;
        public string textoPagos = string.Empty;
        public string textoCotizaciones = string.Empty;
        public string rtCTO = string.Empty;
        public string rtaCE = string.Empty;
        public int cotNum = 1;
        public int numeroFacturaAnterior = 1;
        public string centroCosto = string.Empty;
        public string sRutaCarpeta = ConfigurationManager.AppSettings["RutaArchvoClientes"];


        #endregion

        #region Definiciones

        Pagos pagos = new Pagos();
        Cliente cliente = new Cliente();
        Cotizaciones cotizaciones = new Cotizaciones();
        DataTable tabla;
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
        #endregion

        public Form1()
        {

            InitializeComponent();

        }

        private void btnCerrar_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        #region Funciones
        public void IniciarProceso()
        {

            try
            {
                RegistrarClientes();
            }

            catch (Exception ex)
            {

                MensajeAListBox("Error! " + ex.ToString());
            }
        }
        //INTERFAZ

        public void GenerarArchivoPlano(string texto)
        {


            // Obtener la fecha actual para el nombre de archivo
            DateTime fechaActual = DateTime.Now;
            string rutaFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Registros-FE-" + fechaActual.Day.ToString() + "-" + fechaActual.Month.ToString() + "-" + fechaActual.Year.ToString());
            string nombre = "FacturaElectronica";
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

        public bool InsertarClienteInterfaz(string texto)
        {
            bool ok = false;
            string rta = "";
            rta = FacturacionElectronicaController.InsertarClienteInterfaz(texto);
            if (rta.Equals("OK"))
            {
                ok = true;
            }
            else
            {
                MensajeAListBox(rta.ToString());
                ok = false;
            }
            return ok;

        }

        public bool VerificarClienteExiste(string documento)
        {
            bool ok = false;
            DataTable tabla = new DataTable();
            tabla = FacturacionElectronicaController.ValidarExisteCliente(documento);
            if (tabla.Rows.Count > 0)
            {
                ok = true;
                MensajeAListBox("Cliente se encuentra en la base de datos OK");
            }
            else
            {
                MensajeAListBox("Cliente no se encuentra en la base de datos");
                ok = false;
            }
            return ok;

        }

        public bool InsertarCotizacionesEncabezado(string texto)
        {
            bool ok = false;
            try
            {
                string rtaCE = FacturacionElectronicaController.InsertarCotizacionesEncabezado(textoPagos);
                if (rtaCE.Equals("OK"))
                {
                    MensajeAListBox("Cotizaciones Encabezado guardado correcto OK");
                    GenerarArchivoPlano(textoPagos);
                    MensajeAListBox("Se generó archivo plano cotizaciones encabezado OK");
                    ok = true;


                }
                else
                {
                    MensajeAListBox("No se guardo en la tabla cotizaciones encabezado" + rtaCE.ToString());
                    ok = false;
                }
            }
            catch (Exception ex)
            {

                MensajeAListBox(ex.ToString());
            }
            return ok;
        }

        public bool InsertarCotizaciones(string texto)
        {
            bool ok = false;
            try
            {
                string rtCTO = FacturacionElectronicaController.InsetarPagos(textoCotizaciones);
                if (rtCTO.Equals("OK"))
                {
                    MensajeAListBox("Cotizaciones guardado correcto OK");
                    GenerarArchivoPlano(textoCotizaciones);
                    MensajeAListBox("Se generó archivo plano cotizaciones OK");
                    ok = true;
                    //itemCounter++;

                }
                else
                {
                    MensajeAListBox("No se guardo en la tabla cotizaciones" + rtCTO.ToString());
                    ok = false;
                }
            }
            catch (Exception ex)
            {

                MensajeAListBox("Error insertando Cotizaciones" + ex.ToString());
            }
            return ok;
        }

        public bool ActualizarEstadoPagos(int id)
        {
            bool ok = false;
            try
            {
                string rtaAct = FacturacionElectronicaController.ActualizaEstadoPagos(id);
                if (rtaAct.Equals("OK"))
                {

                    MensajeAListBox("Se actualizó el pago id = " + id);
                    ok = true;
                }
                else
                {
                    MensajeAListBox("No se actulizó el estado del pago");
                    ok = false;
                }
            }
            catch (Exception ex)
            {

                MensajeAListBox("No se actualizó estado pagos " + ex.ToString());
            }
            return ok;
        }

        public void RegistrarPagos()
        {
            //bool ok = false;
            try
            {
                string COE_EMPRESA = "";
                string COE_DOCUMENTO = "";

                //Pagos pagos = new Pagos();
                DataTable tablaPagos;
                tablaPagos = FacturacionElectronicaController.ListarPagos();
                if (tablaPagos.Rows.Count > 0)
                {
                    //tabla = FacturacionElectronicaController.ListarDocumentoVendedor();
                    //foreach (DataRow rtaTabla in tabla.Rows)
                    //{
                    //    cliente.Vendedor = Convert.ToInt32(rtaTabla["VEN_IDENTIFICACION"]);

                    //}


                    int itemCounter = 1;
                    foreach (DataRow registrosPagos in tablaPagos.Rows)
                    {
                        pagos.IdEstacionamiento = Convert.ToInt32(registrosPagos["IdEstacionamiento"]);

                        //Listar datos EmpresaParquearse
                        //DataTable tablaEmpresas = FacturacionElectronicaController.ListarDatosEmpresasPorEstacionamiento(pagos.IdEstacionamiento);
                        //if (tablaEmpresas.Rows.Count > 0)
                        //{
                        //    COE_EMPRESA = tablaEmpresas.Rows[0]["Idc_Empresa"].ToString();
                        //    COE_DOCUMENTO = tablaEmpresas.Rows[0]["DocumentoEmpresa"].ToString();
                        //}

                        DataTable tablaCentroCosto = FacturacionElectronicaController.ListarCentroCosto(pagos.IdEstacionamiento);
                        if (tablaCentroCosto.Rows.Count > 0)
                        {
                            centroCosto = tablaCentroCosto.Rows[0]["CentroCosto"].ToString();
                        }

                        DataTable tablaDocumentoVendedor = FacturacionElectronicaController.ListarDocumentoVendedor();
                        if (tablaDocumentoVendedor.Rows.Count > 0)
                        {
                            cliente.Vendedor = Convert.ToInt32(tablaDocumentoVendedor.Rows[0]["VEN_IDENTIFICACION"]);

                        }
                        pagos.Id = Convert.ToInt32(registrosPagos["Id"]);
                        string empresa = registrosPagos["Empresa"].ToString();
                        pagos.FechaPago = Convert.ToDateTime(registrosPagos["FechaPago"]);
                        //string fechaFormateada = (pagos.Fecha.ToString("dd.MM.yyyy HH.mm");
                        pagos.NumeroDocumento = (registrosPagos["Identificacion"].ToString());
                        string codigoSucursal = registrosPagos["CodigoSucursal"].ToString();
                        pagos.Prefijo = registrosPagos["Prefijo"].ToString();
                        pagos.NumeroFactura = Convert.ToInt32(registrosPagos["NumeroFactura"]);
                        int vendedor = Convert.ToInt32(registrosPagos["Vendedor"]);
                        int totalVenta = Convert.ToInt32(registrosPagos["Total"]);
                        int idTipoPago = Convert.ToInt32(registrosPagos["IdTipoPago"]);
                        string descripcion = Convert.ToString(registrosPagos["TipoPago"]);

                        DataTable tablaCotizaciones;
                        tablaCotizaciones = FacturacionElectronicaController.ListarCotizaciones();
                        if (tablaCotizaciones.Rows.Count > 0)
                        {
                            foreach (DataRow registroCotizaciones in tablaCotizaciones.Rows)
                            {

                                cotizaciones.Cot_Empresa = Convert.ToInt32(registroCotizaciones["COT_EMPRESA"]);
                                cotizaciones.Cot_Documento = Convert.ToString(registroCotizaciones["COT_DOCUMENTO"]);
                                cotizaciones.Cot_Numero = Convert.ToInt32(registroCotizaciones["COT_NUMERO"]);
                                cotizaciones.Cot_Item = Convert.ToInt32(registroCotizaciones["COT_ITEM"]);
                                cotizaciones.Cot_Tipo_Item = Convert.ToInt32(registroCotizaciones["COT_TIPO_ITEM"]);
                                cotizaciones.Cot_Descripcion_Item = Convert.ToString(registroCotizaciones["COT_DESCRIPCION_ITEM"]);
                                cotizaciones.Cot_Referencia = Convert.ToString(registroCotizaciones["COT_REFERENCIA"]);
                                cotizaciones.Cot_Centro_Costo = Convert.ToInt32(registroCotizaciones["COT_CENTRO_COSTO"]);
                                cotizaciones.Cot_Valor_Unitario = Convert.ToInt32(registroCotizaciones["COT_VALOR_UNITARIO"]);

                            }

                            if (numeroFacturaAnterior != pagos.NumeroFactura)
                            {

                                cotNum = cotizaciones.Cot_Numero + 1;

                            }
                            else
                            {
                                cotNum = cotizaciones.Cot_Numero;
                            }

                            //numeroFacturaAnterior = pagos.NumeroFactura;
                        }
                        string referencia = "";
                        if (idTipoPago == 1)
                        {
                            referencia = "05";
                        }
                        else if (idTipoPago == 2)
                        {
                            referencia = "06";
                        }
                        else if (idTipoPago == 3)
                        {
                            referencia = "30";
                        }
                        else if (idTipoPago == 4)
                        {
                            referencia = "31";
                        }
                        else if (idTipoPago == 5)
                        {
                            referencia = "41";
                        }
                        else if (idTipoPago == 6)
                        {
                            referencia = "98";
                        }
                        numeroFacturaAnterior = pagos.NumeroFactura;
                        //DateTime selectedDate = datePicker.Value;
                        //int dateNumber = int.Parse(fecha.ToString("yyyyMMdd"));
                        //fechaFormateada = dateNumber.ToString();

                        //CAMBIAR FECHA A FORMATO NUMERO

                        DateTime fechaHoy = DateTime.Now;
                        string fechaStr = fechaHoy.ToString("yyyy-MM-dd");


                        DateTime fechaNum = DateTime.ParseExact(fechaStr, "yyyy-MM-dd", null);
                        int numeroFecha = (int)(fechaHoy - new DateTime(1899, 12, 30)).TotalDays;
                        string observaciones = "Reemplazo factura POS "+""+pagos.Prefijo+" - " + pagos.NumeroFactura + "";

                        textoPagos = $"INSERT INTO COTIZACION_ENCABEZADO (COE_EMPRESA, COE_DOCUMENTO,COE_NUMERO,COE_FECHA,COE_CLIENTE,COE_CLIENTE_SUCURSAL,COE_SINCRONIZADO,COE_ERRORES,COE_OBSERVACIONES," +
                             $"COE_NUMERO_MG,COE_FECHA_UPDATE,COE_ANTICIPO,COE_FRA_PREFIJO,COE_FRA_NUMERO, COE_DEV_CONCEPTO,COE_VENDEDOR,COE_FORMA_PAGO)" +
                             $"VALUES({empresa},'OF01',{cotNum},{numeroFecha},{pagos.NumeroDocumento},1,0,NULL,'{observaciones}',NULL,NULL,0,NULL,NULL,NULL,'{cliente.Vendedor}',1)";

                        //M/*ensajeAListBox("Registro Numero " + tablaPagos.Rows.Count + " " + textoPagos + "");*/

                        #region Old
                        //FbConnection fbCon = new FbConnection(_ConnectionStringFirebird);
                        //fbCon.Open();
                        //FbCommand comando = new FbCommand(textoPagos, fbCon);
                        //comando.ExecuteNonQuery();
                        //MensajeAListBox("Se guardó un pago OK");
                        #endregion

                        //try
                        //{
                        textoCotizaciones = $"INSERT INTO COTIZACIONES (COT_EMPRESA, COT_DOCUMENTO, COT_NUMERO, COT_ITEM, COT_TIPO_ITEM, COT_DESCRIPCION_ITEM, COT_REFERENCIA, COT_BODEGA," +
                                             $"  COT_CANTIDAD, COT_VALOR_UNITARIO, COT_VR_DTO, COT_FECHA_UPDATE, COT_CENTRO_COSTO, COT_PROYECTO)" +
                                             $" VALUES({empresa},'OF01',{cotNum},{itemCounter},2,'.','{referencia}',NULL,1,{totalVenta},0,NULL,'{centroCosto}',NULL);";
                        //string rtCTO = FacturacionElectronicaController.InsetarPagos(textoCotizaciones);
                        //if (rtCTO.Equals("OK"))
                        //{
                        //    MensajeAListBox("Cotizaciones guardado correcto OK");
                        //    GenerarArchivoPlano(textoCotizaciones);
                        //    MensajeAListBox("Se generó archivo plano cotizaciones OK");
                        //    itemCounter++;
                        if (InsertarCotizaciones(textoCotizaciones))
                        {
                            itemCounter++;
                        }

                        else
                        {
                            MensajeAListBox("Cotizaciones encabezazdo ya está registrado para este pago");
                        }
                        //}
                        //else
                        //{
                        //    MensajeAListBox("No se guardo en la tabla cotizaciones" + rtCTO.ToString());
                        //}

                        //}
                        //catch (Exception ex)
                        //{

                        //    MensajeAListBox(ex.ToString());
                        //}

                        //try
                        //{

                        //}
                        //catch (Exception ex)
                        //{

                        //    MensajeAListBox(ex.ToString());
                        //}
                        ActualizarEstadoPagos(pagos.Id);
                        //string rtaAct = FacturacionElectronicaController.ActualizaEstadoPagos(pagos.Id);
                        //if (rtaAct.Equals("OK"))
                        //{
                        //    MensajeAListBox("Se actualizó el pago id = " + pagos.Id);
                        //}


                    }
                    if (InsertarCotizacionesEncabezado(textoPagos))
                    {

                    }

                }
                else
                {
                    if (ListarClientesNuevos())
                    {
                        notifyIcon1.Icon = SystemIcons.Application;
                        notifyIcon1.BalloonTipText = "Una nuevo cliente solicita registrarse";
                        notifyIcon1.ShowBalloonTip(1000);
                    }
                    else
                    {
                        //this.WindowState = FormWindowState.Minimized;
                        ////this.Hide();

                    }
                }
            }
            catch (Exception ex)
            {

                MensajeAListBox(ex.ToString());
            }

        }

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
        public void RegistrarClientes()
        {
            try
            {
                ////Cliente cliente = new Cliente();
                //bool ok = false;
                //string textCliente = string.Empty;

                //tabla = FacturacionElectronicaController.ListarClientes();
                //if (tabla.Rows.Count > 0)
                //{
                //    foreach (DataRow registrosClientes in tabla.Rows)
                //    {
                //        cliente.Identificacion = (registrosClientes["Identificacion"].ToString());
                //        cliente.RazonSocial = registrosClientes["RazonSocial"].ToString();
                //        cliente.Direccion = registrosClientes["Direccion"].ToString();
                //        cliente.Telefono = registrosClientes["Telefono"].ToString();
                //        cliente.Email = registrosClientes["Email"].ToString();
                //        string ciudad = registrosClientes["Nombre"].ToString();
                //        string fechaSeparada = cliente.Fecha.ToString("dd.MM.yyyy HH.mm");
                //        cliente.Estado = Convert.ToBoolean(registrosClientes["Estado"]);

                //        tabla = FacturacionElectronicaController.ListarDocumentoVendedor();
                //        foreach (DataRow rtaTabla in tabla.Rows)
                //        {
                //            cliente.Vendedor = Convert.ToInt32(rtaTabla["VEN_IDENTIFICACION"]);

                //        }
                //        // Generar el texto del cliente
                //        textoCliente = $"INSERT INTO CLIENTES (CLI_EMPRESA, CLI_IDENTIFICACION, CLI_CODIGO_SUCURSAL, CLI_RAZON_SOCIAL, CLI_DIRECCION, CLI_TELEFONO, " +
                //           $"CLI_EMAIL_FE, CLI_CIUDAD, CLI_VENDEDOR, CLI_CUPO_CREDITO, CLI_FECHA_UPDATE) " +
                //           $"VALUES ('1', '{cliente.Identificacion}', 1, '{cliente.RazonSocial}', '{cliente.Direccion}', " +
                //           $"'{cliente.Telefono}', '{cliente.Email}', '{ciudad}', {cliente.Vendedor}, NULL,NULL)";

                //        MensajeAListBox("Registro Numero " + tabla.Rows.Count + " " + textoCliente + "");

                //        #region Old
                //INSERTAR A LA BD INTERFAZ
                //FbConnection fbCon = new FbConnection(_ConnectionStringFirebird);
                // string eliminar = "DELETE FROM COTIZACION_ENCABEZADO";
                // fbCon.Open();
                // FbCommand comando = new FbCommand(eliminar, fbCon);
                // comando.ExecuteNonQuery();
                // MensajeAListBox("Se guardó un cliente OK");
                // fbCon.Close();
                //        #endregion

                //        if (VerificarClienteExiste(cliente.Identificacion.ToString()))
                //        {
                //            string rtaCliente = "";
                //            rtaCliente = FacturacionElectronicaController.ActualizarEstadoCliente();
                //            if (rtaCliente.Equals("OK"))
                //            {
                //                MensajeAListBox("Se actualizó estado cliente OK");
                //                RegistrarPagos();
                //            }
                //            else
                //            {
                //                MensajeAListBox("Error " + "No se actualizó el cliente");
                //            }

                //        }
                //        else
                //        {
                //            MensajeAListBox("Cliente no existe en la base de datos");

                //            if (InsertarClienteInterfaz(textoCliente))
                //            {
                //                MensajeAListBox("Cliente guardado en la base de datos OK");
                //                string rtaCliente = "";
                //                rtaCliente = FacturacionElectronicaController.ActualizarEstadoCliente();
                //                if (rtaCliente.Equals("OK"))
                //                {
                //                    MensajeAListBox("Se actualizó estado cliente OK");
                //                    GenerarArchivoPlano(textoCliente);
                //                    MensajeAListBox("Se generó el documento .txt OK");
                //                    RegistrarPagos();
                //                }
                //                else
                //                {
                //                    MensajeAListBox("Error " + "No se actualizó el cliente");
                //                }



                //            }
                //        }


                //    }
                //}
                //else
                //{
                RegistrarPagos();

                //}
            }
            catch (Exception ex)
            {

                MensajeAListBox(ex.ToString());
            }
        }

        //LISTADOS
        public void listarCotizacionesEncabezado()
        {
            dataGridView1.DataSource = FacturacionElectronicaController.ListarCotizacionesEncabezado();

        }

        public void listarCotizaciones()
        {
            dataGridView1.DataSource = FacturacionElectronicaController.ListarCotizaciones();

        }

        public void ListarClientesInterfaz()
        {
            dataGridView1.DataSource = FacturacionElectronicaController.ListarClientesInterfaz();
        }

        public bool ListarClientesNuevos()
        {
            //DataTable tabla;
            //bool ok= false; 
            //tabla = FacturacionElectronicaController.ListarClientesNuevos();
            //if (tabla.Rows.Count > 0)
            //{
            //    dataGridView1.DataSource = FacturacionElectronicaController.ListarClientesNuevos();
            //    ok=true;
            //}
            //else
            //{
            //    ok=false;
            //}
            //return ok;
            DataTable tabla;
            bool ok = false;
            tabla = FacturacionElectronicaController.ListarClientesNuevos();
            if (tabla.Rows.Count > 0)
            {
                string valor = Convert.ToString(tabla.Rows[0]["Identificacion"].ToString());
                dataGridView1.DataSource = tabla;
                ok = true;

                // Obtener la fecha actual
                DateTime fechaActual = DateTime.Now;

                // Crear la ruta de la carpeta
                string carpetaFecha = fechaActual.ToString("yyyy-MM-dd");
                sRutaCarpeta = Path.Combine(sRutaCarpeta, carpetaFecha, valor);
                Directory.CreateDirectory(sRutaCarpeta);

                string correo = Convert.ToString(tabla.Rows[0]["Email"].ToString());
                string rut = Convert.ToString(tabla.Rows[0]["Rut"].ToString());

                tabla = FacturacionElectronicaController.ListarClientesNuevosPorDoc(Convert.ToInt32(valor));

                if (tabla.Rows.Count > 0)
                {
                    if (rut != string.Empty)
                    {
                        byte[] pdfData = (byte[])tabla.Rows[0]["Rut"];
                        if (pdfData != null && pdfData.Length > 0)
                        {
                            string nombreArchivoPDF = valor + "-" + correo + ".pdf";
                            string rutaArchivoPDF = Path.Combine(sRutaCarpeta, nombreArchivoPDF);
                            File.WriteAllBytes(rutaArchivoPDF, pdfData);
                            //ActualizaEstadoCliente(Convert.ToInt32(valor));
                        }
                    }
                    else
                    {
                        //ActualizaEstadoCliente(Convert.ToInt32(valor));

                    }
                }

                // Guardar los datos en un archivo de texto vertical
                string nombreArchivoTexto = valor + ".txt";
                string rutaArchivoTexto = Path.Combine(sRutaCarpeta, nombreArchivoTexto);
                GuardarRegistrosEnExcel(rutaArchivoTexto, tabla);
            }
            else
            {
                ok = false;
            }
            return ok;
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

        public void DescargarDocumento()
        {
            if (dataGridView1.Rows.Count > 0)
            {
                string valor = Convert.ToString(dataGridView1.CurrentRow.Cells["Identificacion"].Value);
                string correo = Convert.ToString(dataGridView1.CurrentRow.Cells["Email"].Value);
                string rut = Convert.ToString(dataGridView1.CurrentRow.Cells["rut"].Value);

                tabla = FacturacionElectronicaController.ListarClientesNuevosPorDoc(Convert.ToInt32(valor));

                if (tabla.Rows.Count > 0)
                {
                    if (rut != string.Empty)
                    {

                        byte[] pdfData = (byte[])tabla.Rows[0]["Rut"];
                        if (pdfData != null && pdfData.Length > 0)
                        {
                            SaveFileDialog saveFileDialog = new SaveFileDialog();
                            saveFileDialog.FileName = "" + valor + "-" + correo + ".pdf";
                            saveFileDialog.DefaultExt = ".pdf";
                            saveFileDialog.Filter = "Archivos PDF (*.pdf)|*.pdf";
                            saveFileDialog.Title = "Guardar archivo PDF";

                            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                            {
                                string rutaArchivo = saveFileDialog.FileName;
                                File.WriteAllBytes(rutaArchivo, pdfData);
                                //ActualizaEstadoCliente(Convert.ToInt32(valor));
                            }
                        }
                    }
                }
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

        private void btnIniciar_Click(object sender, EventArgs e)
        {
            IniciarProceso();
        }

        private void lstCotizaciones_Click(object sender, EventArgs e)
        {
            listarCotizaciones();
        }

        private void lstCotEncabezado_Click(object sender, EventArgs e)
        {
            listarCotizacionesEncabezado();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            ListarClientesInterfaz();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DateTime miFecha = DateTime.Now;

            string fechaStr = miFecha.ToString("yyyy-MM-dd");


            DateTime fecha = DateTime.ParseExact(fechaStr, "yyyy-MM-dd", null);
            int numero = (int)(fecha - new DateTime(1899, 12, 30)).TotalDays;

            MensajeAListBox(numero.ToString());
        }

        private void Form1_SizeChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
                notifyIcon1.Icon = SystemIcons.Application;
                notifyIcon1.BalloonTipText = "Consultado Información en segundo plano";
                notifyIcon1.ShowBalloonTip(1000);
            }

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

        private void timer1_Tick(object sender, EventArgs e)
        {
            IniciarProceso();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            MensajeAListBox("Inicia el proceso a la espera de información a sincronizar");
            timer1.Start();
            this.WindowState = FormWindowState.Minimized;
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //listarCotizaciones();
        }

        private void btnDescargarDoc_Click(object sender, EventArgs e)
        {
            DescargarDocumento();
        }

        private void btnEncabezado_Click(object sender, EventArgs e)
        {
            //listarCotizacionesEncabezado();

            //FbConnection fbCon = new FbConnection(_ConnectionStringFirebird);
            //string eliminar = "DELETE FROM COTIZACIONES";
            //fbCon.Open();
            //FbCommand comando = new FbCommand(eliminar, fbCon);
            //comando.ExecuteNonQuery();
            //MensajeAListBox("Se guardó un cliente OK");
            //fbCon.Close();
        }

        private void button1_Click_2(object sender, EventArgs e)
        {
            listarCotizaciones();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                string valor = Convert.ToString(dataGridView1.CurrentRow.Cells["Identificacion"].Value);
                string correo = Convert.ToString(dataGridView1.CurrentRow.Cells["Email"].Value);
                string rut = Convert.ToString(dataGridView1.CurrentRow.Cells["rut"].Value);

                tabla = FacturacionElectronicaController.ListarClientesNuevosPorDoc(Convert.ToInt32(valor));

                if (tabla.Rows.Count > 0)
                {

                    ActualizaEstadoCliente(Convert.ToInt32(valor));
                    dataGridView1.Rows.Clear();
                }
            }
        }
    }
}
        
    


