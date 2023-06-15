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
                        MensajeAListBox("Registro Numero " + tabla.Rows.Count + " " + textoCliente + "");

                        // INSERTAR A LA BD INTERFAZ
                        FbConnection fbCon = new FbConnection(_ConnectionStringFirebird);
                        fbCon.Open();
                        FbCommand comando = new FbCommand(textoCliente, fbCon);
                        comando.ExecuteNonQuery();

                        MensajeAListBox("Se guardó un cliente OK");
                        fbCon.Close();

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
                        Pagos pagos = new Pagos();
                        DataTable tablaPagos;
                        tablaPagos = FacturacionElectronicaController.ListarPagos();
                        if (tablaPagos.Rows.Count > 0)
                        {
                            foreach (DataRow registrosClientes in tablaPagos.Rows)
                            {
                                string empresa = tablaPagos.Rows[0]["Empresa"].ToString();
                                DateTime fecha = Convert.ToDateTime(tablaPagos.Rows[0]["Fecha"]);
                                pagos.NumeroDocumento = Convert.ToInt32(tablaPagos.Rows[0]["Identificacion"]);
                                string codigoSucursal = tablaPagos.Rows[0]["CodigoSucursal"].ToString();
                                pagos.Prefijo = tablaPagos.Rows[0]["Prefijo"].ToString();
                                pagos.NumeroFactura = Convert.ToInt32(tablaPagos.Rows[0]["NumeroFactura"]);
                                int vendedor = Convert.ToInt32(tablaPagos.Rows[0]["Vendedor"]);

                                textoPagos = $"INSERT INTO COTIZACIONES (COE_EMPRESA, COE_DOCUMENTO,COE_NUMERO,COE_FECHA,COE_CLIENTE,COE_CLIENTE_SUCURSAL,COE_SINCRONIZADO,COE_ERRORES,COE_OBSERVACIONES," +
                                    $"COE_NUMERO_MG,COE_FECHA_UPDATE,COE_ANTICIPO,COE_FRA_PREFIJO,COE_FRA_NUMERO, COE_DEV_CONCEPTO,COE_VENDEDOR)" +
                                    $"VALUES('1',{pagos.NumeroDocumento},1,{pagos.Fecha},1,1,1,'NULL','NULL',1,'NULL',0,{pagos.Prefijo},{pagos.NumeroFactura},'NULL','NULL')";

                            MensajeAListBox("Registro Numero " + tablaPagos.Rows.Count + " " + textoPagos + "");

                            FbConnection fbCon = new FbConnection(_ConnectionStringFirebird);
                                fbCon.Open();
                                FbCommand comando = new FbCommand(textoPagos, fbCon);
                                comando.ExecuteNonQuery();
                             MensajeAListBox("Se guardó un pago OK");

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
                                fbCon.Close();
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
