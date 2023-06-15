using Controlador;
using FirebirdSql.Data.FirebirdClient;
using FirebirdSql.Data.Services;
using Microsoft.Win32;
using Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.Common;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Web.Handlers;

namespace FacturacionElectronicaServices
{
    public partial class FacturacionElectronicaServices : ServiceBase
    {
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
        string textoCliente = string.Empty;
        string textPagos=string.Empty; 
        private System.Timers.Timer oTimer;
        private static object objLock = new object();
        private static FacturacionElectronicaServices Agente = new FacturacionElectronicaServices();
        private int _UpdateTimeBegin
        {
            get
            {
                string sSerial = ConfigurationManager.AppSettings["HourUpdateTimeBegin"];
                if (string.IsNullOrEmpty(sSerial))
                {
                    return 4;
                }
                else
                {
                    return Convert.ToInt32(sSerial);
                }
            }
        }

        static void Main(string[] args)
        {

            Process.GetCurrentProcess().Exited += new EventHandler(FacturacionElectronicaServices_Exited);

            try
            {
                if (System.Diagnostics.Process.GetProcessesByName
                    (System.Diagnostics.Process.GetCurrentProcess().ProcessName).Length > 1)
                    throw new ApplicationException("Existe otra instancia del servicio en ejecución.");

                if (!Environment.UserInteractive)
                    FacturacionElectronicaServices.Run(Agente);
                else
                {
                    if (Environment.UserInteractive)
                        Console.ForegroundColor = ConsoleColor.Green;

                    Agente.OnStart(null);

                    if (Environment.UserInteractive)
                        Console.ForegroundColor = ConsoleColor.Green;

                    if (Environment.UserInteractive)
                        Console.WriteLine("El servicio se inicio correctamente y queda en espera de atender solicitudes.");

                    System.Threading.Thread.Sleep(System.Threading.Timeout.Infinite);
                }
            }
            catch (Exception ex)
            {
                if (Environment.UserInteractive)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("Ocurrió un error iniciando el servicio.");
                    Console.WriteLine(ex.Message);
                    System.Threading.Thread.Sleep(new TimeSpan(0, 1, 0));
                }
            }
        }
        public FacturacionElectronicaServices()
        {
            oTimer = new Timer(2 * 1000);
            oTimer.Elapsed += new ElapsedEventHandler(oTimer_Elapsed);
            InitializeComponent();
        }


        protected override void OnStart(string[] args)
        {
            oTimer.Enabled = true;
        }

        protected override void OnStop()
        {
            oTimer.Enabled = false;
        }
        void oTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            oTimer.Enabled = false;

            MensajeAListBox("El servicio inicia integracion");

            try
            {
                lock (objLock)
                {
                    if (ListarClientes())
                    {
                        ActualizarEstadoClientes();
                    }

                    if (ListarPagos())
                    {
                        ActualizarEstadoPagos();
                    }

                }
            }
            catch (Exception ex)
            {
                MensajeAListBox("Excepcion: " + ex.InnerException + " " + ex.Message + " " + ex.Source);
                oTimer.Enabled = true;
            }

            oTimer.Enabled = true;
        }

        #region Funciones

        public bool ListarClientes()
        {
            Cliente cliente = new Cliente();
            bool ok = false;
            string textCliente = string.Empty;
            DataTable tabla;
            tabla = FacturacionElectronicaController.ListarClientes();
            if (tabla.Rows.Count > 0)
            {
                foreach (DataRow registrosClientes in tabla.Rows)
                {
                    cliente.Identificacion = Convert.ToInt32(registrosClientes["Identificacion"]);
                    cliente.RazonSocial = registrosClientes["RazonSocial"].ToString();
                    cliente.Direccion = registrosClientes["Direccion"].ToString();
                    cliente.Telefono = registrosClientes["Telefono"].ToString();
                    cliente.Email = registrosClientes["Email"].ToString();
                    string ciudad = registrosClientes["Nombre"].ToString();
                    cliente.Fecha = Convert.ToDateTime(registrosClientes["Fecha"]);
                    cliente.Estado = Convert.ToBoolean(registrosClientes["Estado"]);

                    // Generar el texto del cliente
                     textoCliente = $"INSERT INTO CLIENTES (CLI_EMPRESA, CLI_IDENTIFICACION, CLI_CODIGO_SUCURSAL, CLI_RAZON_SOCIAL, CLI_DIRECCION, CLI_TELEFONO, " +
                        $"CLI_EMAIL_FE, CLI_CIUDAD, CLI_VENDEDOR, CLI_CUPO_CREDITO, CLI_FECHA_UPDATE) " +
                        $"VALUES ('1', '{cliente.Identificacion}', 0, '{cliente.RazonSocial}', '{cliente.Direccion}', " +
                        $"'{cliente.Telefono}', '{cliente.Email}', '{cliente.IdCiudad}', '1', 0, '{cliente.Fecha}')";

                    // INSERTAR A LA BD INTERFAZ
                    FbConnection fbCon = new FbConnection(_ConnectionStringFirebird);
                    fbCon.Open();
                    FbCommand comando = new FbCommand(textoCliente, fbCon);
                    comando.ExecuteNonQuery();

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
                        sw.WriteLine(textoCliente);
                    }
                    fbCon.Close();

                }
                string rta = "";
                rta = FacturacionElectronicaController.ActualizarEstadoCliente();
                if (rta.Equals("OK"))
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

            return ok;
        }       

        public bool ListarPagos()
        {
            bool ok = false;
            Pagos pagos = new Pagos();
            DataTable tabla;
            tabla = FacturacionElectronicaController.ListarPagos();
            if (tabla.Rows.Count > 0)
            {
                foreach (DataRow registrosClientes in tabla.Rows)
                {
                    string empresa = tabla.Rows[0]["Empresa"].ToString();
                    DateTime fecha = Convert.ToDateTime(tabla.Rows[0]["Fecha"]);
                    pagos.NumeroDocumento = Convert.ToInt32(tabla.Rows[0]["Identificacion"]);
                    string codigoSucursal = tabla.Rows[0]["CodigoSucursal"].ToString();
                    pagos.Prefijo = tabla.Rows[0]["Prefijo"].ToString();
                    pagos.NumeroFactura = Convert.ToInt32(tabla.Rows[0]["NumeroFactura"]);
                    int vendedor = Convert.ToInt32(tabla.Rows[0]["Vendedor"]);

                    textPagos =$"INSERT INTO COTIZACIONES (COE_EMPRESA, COE_DOCUMENTO,COE_NUMERO,COE_FECHA,COE_CLIENTE,COE_CLIENTE_SUCURSAL,COE_SINCRONIZADO,COE_ERRORES,COE_OBSERVACIONES," +
                        $"COE_NUMERO_MG,COE_FECHA_UPDATE,COE_ANTICIPO,COE_FRA_PREFIJO,COE_FRA_NUMERO, COE_DEV_CONCEPTO,COE_VENDEDOR)" +
                        $"VALUES('1',{pagos.NumeroDocumento},1,{pagos.Fecha},1,1,1,'NULL','NULL',1,'NULL',0,{pagos.Prefijo},{pagos.NumeroFactura},'NULL','NULL')";

                    FbConnection fbCon = new FbConnection(_ConnectionStringFirebird);
                    fbCon.Open();
                    FbCommand comando = new FbCommand(textPagos, fbCon);
                    comando.ExecuteNonQuery();

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
                        sw.WriteLine(textPagos);
                    }
                    fbCon.Close();

                    ok = true;
                }
            }
            else
            {
                ok = false;
            }
   
            return ok;
        }

        public bool ActualizarEstadoClientes()
        {
            bool ok = false;
            string rta = "";
            try
            {
                rta = FacturacionElectronicaController.ActualizarEstadoCliente();
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

        public bool ActualizarEstadoPagos()
        {
            bool ok = false;
            string rta = "";
            try
            {
                rta = FacturacionElectronicaController.ActualizaEstadoPagos();
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
        //GENERAR SCRIPTS CLIENTES

        public void GenerarArchivoPlano()
        {
            DateTime MyDate = new DateTime();
            string rutaFolder = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Registros-" + MyDate.Day.ToString() + "-" + MyDate.Month.ToString() + "-" + MyDate.Year.ToString());
            string nombre = "FacturaElectronica";
            string path = rutaFolder + "/" + nombre + @".txt";

        }

        private void MensajeAListBox(string mensaje)
        {
            //lbEventos.Items.Add(DateTime.Now.ToString("dd/MM/yy HH:mm:ss") + " -> " + mensaje);
            //this.lbEventos.SelectedIndex = this.lbEventos.Items.Count - 1;
            //TraceHandler.WriteLine(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @"Logs\Log"), "MENSAJE: " + mensaje, TipoLog.TRAZA);
        }



        #endregion

        static void FacturacionElectronicaServices_Exited(object sender, EventArgs e)
        {
            try
            {
                if (!Environment.UserInteractive)
                {
                    Agente.OnStop();
                    Agente = null;
                }
            }
            catch { }
        }
    }
}
