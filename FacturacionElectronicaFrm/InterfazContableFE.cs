using Controlador;
using FirebirdSql.Data.FirebirdClient;
using Modelo;
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
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

        #endregion

        #region Definiciones

        Pagos pagos = new Pagos();
        Cliente cliente = new Cliente();
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
                //Cliente cliente = new Cliente();
                bool ok = false;
                string textCliente = string.Empty;
                DataTable tabla;
                tabla = FacturacionElectronicaController.ListarClientes();
                if (tabla.Rows.Count > 0)
                {
                    foreach (DataRow registrosClientes in tabla.Rows)
                    {
                        cliente.NumeroDocumento = (registrosClientes["NumeroDocumento"].ToString());
                        cliente.RazonSocial = registrosClientes["RazonSocial"].ToString();
                        cliente.Direccion = registrosClientes["Direccion"].ToString();
                        cliente.Telefono = registrosClientes["Telefono"].ToString();
                        cliente.Email = registrosClientes["Email"].ToString();
                        string ciudad = registrosClientes["Nombre"].ToString();
                        string fechaSeparada = cliente.Fecha.ToString("dd.MM.yyyy HH.mm");
                        cliente.Estado = Convert.ToBoolean(registrosClientes["Estado"]);

                        tabla = FacturacionElectronicaController.ListarDocumentoVendedor();
                        foreach (DataRow rtaTabla in tabla.Rows)
                        {
                            cliente.Vendedor = Convert.ToInt32(rtaTabla["VEN_IDENTIFICACION"]);

                        }


                        // Generar el texto del cliente
                        textoCliente = $"INSERT INTO CLIENTES (CLI_EMPRESA, CLI_IDENTIFICACION, CLI_CODIGO_SUCURSAL, CLI_RAZON_SOCIAL, CLI_DIRECCION, CLI_TELEFONO, " +
                           $"CLI_EMAIL_FE, CLI_CIUDAD, CLI_VENDEDOR, CLI_CUPO_CREDITO, CLI_FECHA_UPDATE) " +
                           $"VALUES ('1', '{cliente.NumeroDocumento}', 0, '{cliente.RazonSocial}', '{cliente.Direccion}', " +
                           $"'{cliente.Telefono}', '{cliente.Email}', '{ciudad}', {cliente.Vendedor}, 0.00,'{fechaSeparada}')";
                        MensajeAListBox("Registro Numero " + tabla.Rows.Count + " " + textoCliente + "");


                        #region Old
                        // INSERTAR A LA BD INTERFAZ
                        //FbConnection fbCon = new FbConnection(_ConnectionStringFirebird);
                        //string eliminar = "UPDATE COTIZACIONES SET COT_DOCUMENTO='OF01' WHERE COT_DOCUMENTO='FV09'";
                        //fbCon.Open();
                        //FbCommand comando = new FbCommand(eliminar, fbCon);
                        //comando.ExecuteNonQuery();
                        //MensajeAListBox("Se guardó un cliente OK");
                        //fbCon.Close();
                        #endregion

                        if (VerificarClienteExiste(cliente.NumeroDocumento.ToString()))
                        {
                            string rtaCliente = "";
                            rtaCliente = FacturacionElectronicaController.ActualizarEstadoCliente();
                            if (rtaCliente.Equals("OK"))
                            {
                                MensajeAListBox("Se actualizó estado cliente OK");
                            }
                            else
                            {
                                MensajeAListBox("Error " + "No se actualizó el cliente");
                            }
                        }
                        else
                        {
                            MensajeAListBox("Cliente no existe en la base de datos");

                            if (InsertarClienteInterfaz(textoCliente))
                            {
                                MensajeAListBox("Cliente guardado en la base de datos OK");
                                string rtaCliente = "";
                                rtaCliente = FacturacionElectronicaController.ActualizarEstadoCliente();
                                if (rtaCliente.Equals("OK"))
                                {
                                    MensajeAListBox("Se actualizó estado cliente OK");
                                    GenerarArchivoPlano(textoCliente);
                                    MensajeAListBox("Se generó el documento .txt OK");
                                }
                                else
                                {
                                    MensajeAListBox("Error " + "No se actualizó el cliente");
                                }

                                string COE_EMPRESA = "";
                                string COE_DOCUMENTO = "";

                                //Pagos pagos = new Pagos();
                                DataTable tablaPagos;
                                tablaPagos = FacturacionElectronicaController.ListarPagos();
                                if (tablaPagos.Rows.Count > 0)
                                {
                                    int itemCounter = 1;
                                    foreach (DataRow registrosPagos in tablaPagos.Rows)
                                    {
                                        pagos.IdEstacionamiento = Convert.ToInt32(tablaPagos.Rows[0]["IdEstacionamiento"]);

                                        //Listar datos EmpresaParquearse
                                        DataTable tablaEmpresas = FacturacionElectronicaController.ListarDatosEmpresasPorEstacionamiento(pagos.IdEstacionamiento);
                                        if (tablaEmpresas.Rows.Count > 0)
                                        {
                                            COE_EMPRESA = tablaEmpresas.Rows[0]["Idc_Empresa"].ToString();
                                            COE_DOCUMENTO = tablaEmpresas.Rows[0]["DocumentoEmpresa"].ToString();
                                        }

                                        string empresa = tablaPagos.Rows[0]["Empresa"].ToString();
                                        DateTime fecha = Convert.ToDateTime(tablaPagos.Rows[0]["Fecha"]);
                                        string fechaFormateada = fecha.ToString("dd.MM.yyyy HH.mm");
                                        pagos.NumeroDocumento = (tablaPagos.Rows[0]["NumeroDocumento"].ToString());
                                        string codigoSucursal = tablaPagos.Rows[0]["CodigoSucursal"].ToString();
                                        pagos.Prefijo = tablaPagos.Rows[0]["Prefijo"].ToString();
                                        pagos.NumeroFactura = Convert.ToInt32(tablaPagos.Rows[0]["NumeroFactura"]);
                                        int vendedor = Convert.ToInt32(tablaPagos.Rows[0]["Vendedor"]);
                                        int totalVenta = Convert.ToInt32(tablaPagos.Rows[0]["Total"]);

                                        string descripcion = Convert.ToString(tablaPagos.Rows[0]["TipoPago"]);

                                        textoPagos = $"INSERT INTO COTIZACION_ENCABEZADO (COE_EMPRESA, COE_DOCUMENTO,COE_NUMERO,COE_FECHA,COE_CLIENTE,COE_CLIENTE_SUCURSAL,COE_SINCRONIZADO,COE_ERRORES,COE_OBSERVACIONES," +
                                $"COE_NUMERO_MG,COE_FECHA_UPDATE,COE_ANTICIPO,COE_FRA_PREFIJO,COE_FRA_NUMERO, COE_DEV_CONCEPTO,COE_VENDEDOR)" +
                                $"VALUES({empresa},'{COE_DOCUMENTO}',0,NULL,{pagos.NumeroDocumento},0,1,NULL,NULL,1,'{fechaFormateada}',0,'{pagos.Prefijo}',{pagos.NumeroFactura},NULL,'{cliente.Vendedor}')";

                                        //M/*ensajeAListBox("Registro Numero " + tablaPagos.Rows.Count + " " + textoPagos + "");*/

                                        #region Old
                                        //FbConnection fbCon = new FbConnection(_ConnectionStringFirebird);
                                        //fbCon.Open();
                                        //FbCommand comando = new FbCommand(textoPagos, fbCon);
                                        //comando.ExecuteNonQuery();
                                        //MensajeAListBox("Se guardó un pago OK");
                                        #endregion

                                        try
                                        {
                                            textoCotizaciones = $"INSERT INTO COTIZACIONES (COT_EMPRESA, COT_DOCUMENTO,COT_NUMERO, COT_ITEM, COT_TIPO_ITEM, COT_DESCRIPCION_ITEM, COT_REFERENCIA, COT_BODEGA," +
                                                                 $"  COT_CANTIDAD, COT_VALOR_UNITARIO, COT_VR_DTO, COT_FECHA_UPDATE, COT_CENTRO_COSTO, COT_PROYECTO)" +
                                                                 $" VALUES({empresa},'{COE_DOCUMENTO}',{COE_EMPRESA},{itemCounter},1,'{descripcion}',NULL,NULL,1,{totalVenta},0,NULL,{pagos.IdEstacionamiento},NULL);";
                                             rtCTO = FacturacionElectronicaController.InsetarPagos(textoCotizaciones);
                                            if (rtCTO.Equals("OK"))
                                            {
                                                MensajeAListBox("Cotizaciones guardado correcto OK");
                                                GenerarArchivoPlano(textoCotizaciones);
                                                itemCounter++;
                                            }
                                            else
                                            {
                                                MensajeAListBox("No se guardo en la tabla cotizaciones" + rtCTO.ToString());
                                            }

                                        }
                                        catch (Exception ex)
                                        {

                                            MensajeAListBox(ex.ToString());
                                        }

                                        try
                                        {
                                             rtaCE = FacturacionElectronicaController.InsertarCotizacionesEncabezado(textoPagos);
                                            if (rtaCE.Equals("OK"))
                                            {
                                                MensajeAListBox("Cotizaciones Encabezado guardado correcto OK");

                                                GenerarArchivoPlano(textoPagos);
                                            }
                                            else
                                            {
                                                MensajeAListBox("No se guardo en la tabla cotizaciones encabezado" + rtaCE.ToString());
                                            }
                                        }
                                        catch (Exception ex)
                                        {

                                            MensajeAListBox(ex.ToString());
                                        }
                                    }
                                }
                                else
                                {
                                    Application.Exit();
                                }

                            }
                        }


                    }
                }
                else
                {
                    string COE_EMPRESA = "";
                    string COE_DOCUMENTO = "";

                    //Pagos pagos = new Pagos();
                    DataTable tablaPagos;
                    tablaPagos = FacturacionElectronicaController.ListarPagos();
                    if (tablaPagos.Rows.Count > 0)
                    {
                        tabla = FacturacionElectronicaController.ListarDocumentoVendedor();
                        foreach (DataRow rtaTabla in tabla.Rows)
                        {
                            cliente.Vendedor = Convert.ToInt32(rtaTabla["VEN_IDENTIFICACION"]);

                        }
                        int itemCounter = 1;
                        foreach (DataRow registrosPagos in tablaPagos.Rows)
                        {
                            pagos.IdEstacionamiento = Convert.ToInt32(tablaPagos.Rows[0]["IdEstacionamiento"]);

                            //Listar datos EmpresaParquearse
                            DataTable tablaEmpresas = FacturacionElectronicaController.ListarDatosEmpresasPorEstacionamiento(pagos.IdEstacionamiento);
                            if (tablaEmpresas.Rows.Count > 0)
                            {
                                COE_EMPRESA = tablaEmpresas.Rows[0]["Idc_Empresa"].ToString();
                                COE_DOCUMENTO = tablaEmpresas.Rows[0]["DocumentoEmpresa"].ToString();
                            }
                            pagos.Id = Convert.ToInt32(registrosPagos["Id"]);
                            string empresa = registrosPagos["Empresa"].ToString();
                            DateTime fecha = Convert.ToDateTime(registrosPagos["Fecha"]);
                            string fechaFormateada = fecha.ToString("dd.MM.yyyy HH.mm");
                            pagos.NumeroDocumento = (registrosPagos["NumeroDocumento"].ToString());
                            string codigoSucursal = registrosPagos["CodigoSucursal"].ToString();
                            pagos.Prefijo = registrosPagos["Prefijo"].ToString();
                            pagos.NumeroFactura = Convert.ToInt32(registrosPagos["NumeroFactura"]);
                            int vendedor = Convert.ToInt32(registrosPagos["Vendedor"]);
                            int totalVenta = Convert.ToInt32(registrosPagos["Total"]);

                            string descripcion = Convert.ToString(registrosPagos["TipoPago"]);

                            textoPagos = $"INSERT INTO COTIZACION_ENCABEZADO (COE_EMPRESA, COE_DOCUMENTO,COE_NUMERO,COE_FECHA,COE_CLIENTE,COE_CLIENTE_SUCURSAL,COE_SINCRONIZADO,COE_ERRORES,COE_OBSERVACIONES," +
                                $"COE_NUMERO_MG,COE_FECHA_UPDATE,COE_ANTICIPO,COE_FRA_PREFIJO,COE_FRA_NUMERO, COE_DEV_CONCEPTO,COE_VENDEDOR)" +
                                $"VALUES({empresa},'{COE_DOCUMENTO}',{itemCounter},NULL,{pagos.NumeroDocumento},0,1,NULL,NULL,1,'{fechaFormateada}',0,'{pagos.Prefijo}',{pagos.NumeroFactura},NULL,'{cliente.Vendedor}')";

                            //M/*ensajeAListBox("Registro Numero " + tablaPagos.Rows.Count + " " + textoPagos + "");*/

                            #region Old
                            //FbConnection fbCon = new FbConnection(_ConnectionStringFirebird);
                            //fbCon.Open();
                            //FbCommand comando = new FbCommand(textoPagos, fbCon);
                            //comando.ExecuteNonQuery();
                            //MensajeAListBox("Se guardó un pago OK");
                            #endregion

                            try
                            {
                                textoCotizaciones = $"INSERT INTO COTIZACIONES (COT_EMPRESA, COT_DOCUMENTO, COT_NUMERO, COT_ITEM, COT_TIPO_ITEM, COT_DESCRIPCION_ITEM, COT_REFERENCIA, COT_BODEGA," +
                                                     $"  COT_CANTIDAD, COT_VALOR_UNITARIO, COT_VR_DTO, COT_FECHA_UPDATE, COT_CENTRO_COSTO, COT_PROYECTO)" +
                                                     $" VALUES({empresa},'{COE_DOCUMENTO}',{COE_EMPRESA},{itemCounter},1,'{descripcion}',NULL,NULL,1,{totalVenta},0,NULL,{pagos.IdEstacionamiento},NULL);";
                                string rtCTO = FacturacionElectronicaController.InsetarPagos(textoCotizaciones);
                                if (rtCTO.Equals("OK"))
                                {
                                    MensajeAListBox("Cotizaciones guardado correcto OK");
                                    GenerarArchivoPlano(textoCotizaciones);
                                    MensajeAListBox("Se generó archivo plano cotizaciones OK");
                                    itemCounter++;

                                }
                                else
                                {
                                    MensajeAListBox("No se guardo en la tabla cotizaciones" + rtCTO.ToString());
                                }

                            }
                            catch (Exception ex)
                            {

                                MensajeAListBox(ex.ToString());
                            }

                            try
                            {
                                string rtaCE = FacturacionElectronicaController.InsertarCotizacionesEncabezado(textoPagos);
                                if (rtaCE.Equals("OK"))
                                {
                                    MensajeAListBox("Cotizaciones Encabezado guardado correcto OK");
                                    GenerarArchivoPlano(textoPagos);
                                    MensajeAListBox("Se generó archivo plano cotizaciones encabezado OK");


                                }
                                else
                                {
                                    MensajeAListBox("No se guardo en la tabla cotizaciones encabezado" + rtaCE.ToString());
                                }
                            }
                            catch (Exception ex)
                            {

                                MensajeAListBox(ex.ToString());
                            }

                            string rtaAct = FacturacionElectronicaController.ActualizaEstadoPagos(pagos.Id);
                            if (rtaAct.Equals("OK"))
                            {
                                MensajeAListBox("Se actualizó el pago id = " + pagos.Id);
                            }
                                
                    
                        }
                    
                    }
                    else
                    {
                        Application.Exit();
                    }
                }
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
                ok= true;
            }
            else
            {
                MensajeAListBox(rta.ToString());
                ok= false;
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
    }

}
