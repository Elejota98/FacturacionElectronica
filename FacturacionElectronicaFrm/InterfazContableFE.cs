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
        #endregion

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
        public void ListarClientes()
        {
            try
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
                        cliente.NumeroDocumento = (registrosClientes["NumeroDocumento"].ToString());
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
                           $"VALUES ('1', '{cliente.NumeroDocumento}', 0, '{cliente.RazonSocial}', '{cliente.Direccion}', " +
                           $"'{cliente.Telefono}', '{cliente.Email}', '{cliente.IdCiudad}', '1', 0, '{cliente.Fecha}')";
                        MensajeAListBox("Registro Numero " + tabla.Rows.Count + " " + textoCliente + "");

                        #region Old
                        // INSERTAR A LA BD INTERFAZ
                        //FbConnection fbCon = new FbConnection(_ConnectionStringFirebird);
                        //fbCon.Open();
                        //FbCommand comando = new FbCommand(textoCliente, fbCon);
                        //comando.ExecuteNonQuery();

                        //MensajeAListBox("Se guardó un cliente OK");
                        //fbCon.Close();
                        #endregion

                        if (VerificarClienteExiste(cliente.NumeroDocumento.ToString()))
                        {
                            // INSERTA PAGOS EN LA TABLA COTIZACIONES Y COTIZACIONES ENCABEZADO


                        }

                        InsertarClienteInterfaz(textoCliente);

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
                        MensajeAListBox("Se generó el documento .txt OK");

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
                    string COE_EMPRESA = "";
                    string COE_DOCUMENTO = "";

                        Pagos pagos = new Pagos();
                        DataTable tablaPagos;
                        tablaPagos = FacturacionElectronicaController.ListarPagos();
                        if (tablaPagos.Rows.Count > 0)
                        {
                            foreach (DataRow registrosClientes in tablaPagos.Rows)
                            {
                            pagos.IdEstacionamiento = Convert.ToInt32(tablaPagos.Rows[0]["IdEstacionamiento"]);

                            //Listar datos EmpresaParquearse
                            DataTable tablaEmpresas = FacturacionElectronicaController.ListarDatosEmpresasPorEstacionamiento(pagos.IdEstacionamiento);
                            if (tablaEmpresas.Rows.Count > 0)
                            {
                                 //COE_EMPRESA = tablaEmpresas.Rows[0]["Idc_Empresa"].ToString();
                                 COE_DOCUMENTO = tablaEmpresas.Rows[0]["DocumentoEmpresa"].ToString();
                            }

                                string empresa = tablaPagos.Rows[0]["Empresa"].ToString();
                                DateTime fecha = Convert.ToDateTime(tablaPagos.Rows[0]["Fecha"]);
                                pagos.NumeroDocumento = (tablaPagos.Rows[0]["NumeroDocumento"].ToString());
                                string codigoSucursal = tablaPagos.Rows[0]["CodigoSucursal"].ToString();
                                pagos.Prefijo = tablaPagos.Rows[0]["Prefijo"].ToString();
                                pagos.NumeroFactura = Convert.ToInt32(tablaPagos.Rows[0]["NumeroFactura"]);
                                int vendedor = Convert.ToInt32(tablaPagos.Rows[0]["Vendedor"]);

                                textoPagos = $"INSERT INTO COTIZACION_ENCABEZADO (COE_EMPRESA, COE_DOCUMENTO,COE_NUMERO,COE_FECHA,COE_CLIENTE,COE_CLIENTE_SUCURSAL,COE_SINCRONIZADO,COE_ERRORES,COE_OBSERVACIONES," +
                                    $"COE_NUMERO_MG,COE_FECHA_UPDATE,COE_ANTICIPO,COE_FRA_PREFIJO,COE_FRA_NUMERO, COE_DEV_CONCEPTO,COE_VENDEDOR)" +
                                    $"VALUES({empresa},{COE_DOCUMENTO},1,{pagos.Fecha},1,1,1,'NULL','NULL',1,'NULL',0,{pagos.Prefijo},{pagos.NumeroFactura},'NULL','NULL')";

                                MensajeAListBox("Registro Numero " + tablaPagos.Rows.Count + " " + textoPagos + "");

                            #region Old
                            //FbConnection fbCon = new FbConnection(_ConnectionStringFirebird);
                            //fbCon.Open();
                            //FbCommand comando = new FbCommand(textoPagos, fbCon);
                            //comando.ExecuteNonQuery();
                            //MensajeAListBox("Se guardó un pago OK");
                            #endregion

                            try
                            {
                                string rtaCE = FacturacionElectronicaController.InsertarCotizacionesEncabezado(textoPagos);
                                if (rtaCE.Equals("OK"))
                                {
                                    MensajeAListBox("Cotizaciones Encabezado guardado correcto OK");
                                }
                                else
                                {
                                    MensajeAListBox("No se guardo en la tabla cotizaciones encabezado");
                                }
                            }
                            catch (Exception ex )
                            {

                                MensajeAListBox(ex.ToString());
                            }


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
                                    sw.WriteLine(textoPagos);

                                MensajeAListBox("Se generó el documento .txt OK");
                                }
                               
                            }

                        string rta = "";
                            rta = FacturacionElectronicaController.ActualizarEstadoCliente();
                            if (rta.Equals("OK"))
                            {
                            MensajeAListBox("Se Actualizó el cliente");
                            }
                            else
                            {
                            MensajeAListBox( "Error "+"No se actualizó el cliente");
                            }
                    }
                        else
                    {
                        Application.Exit();
                    }

                    }

                }            
       
            catch (Exception ex )
            {

                MensajeAListBox("Error! " + ex.ToString());
            }

        }


        //INTERFAZ

        public void InsertarClienteInterfaz(string texto)
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
            ListarClientes();
            {
                //ActualizarEstadoClientes();
            }
        }
    }

}
