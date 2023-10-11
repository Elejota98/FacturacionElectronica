using Modelo;
using Servicios;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;

namespace Controlador
{
    public class FacturacionElectronicaController
    {
        public static DataTable ListarClientes()
        {
            RepositorioFacturacionElectronica Datos = new RepositorioFacturacionElectronica();
            return Datos.ListarClientes();
        }

        public static string ActualizarEstadoCliente()
        {
            RepositorioFacturacionElectronica Datos = new RepositorioFacturacionElectronica();
            return Datos.ActualizaClientes();
        }

        public static DataTable ListarPagos()
        {
            RepositorioFacturacionElectronica Datos = new RepositorioFacturacionElectronica();
            return Datos.ListarPagos();
        }
        public static DataTable ListarClientesNuevos()
        {
            RepositorioFacturacionElectronica Datos = new RepositorioFacturacionElectronica();
            return Datos.ListarClientesNuevos();
        }
        public static DataTable ListarClientesNuevosPorDoc(int identificacion)
        {
            RepositorioFacturacionElectronica Datos = new RepositorioFacturacionElectronica();
            return Datos.ListarClientesNuevosPorDoc(identificacion);
        }
        //public static string ActualizaEstadoCliente(int identificacion)
        //{
        //    RepositorioFacturacionElectronica Datos = new RepositorioFacturacionElectronica();
        //    return Datos.ActualizaEstadoCliente(identificacion);

        //}

        //INTERFAZ


        #region Cliente
        //public static string InsertarClienteInterfaz(string texto)
        //{
        //    RepositorioFacturacionElectronica Datos = new RepositorioFacturacionElectronica();
        //    return Datos.InsertarClienteInterfaz(texto);
        //}


        public static DataTable ListarDocumentoVendedor()
        {
            RepositorioFacturacionElectronica Datos = new RepositorioFacturacionElectronica();
            return Datos.ListarDocumentoVendedor();
        }

        #region New
        public static string InsertarClienteInterfaz(Cliente cliente)
        {
            string rta = "";
            RepositorioFacturacionElectronica Datos = new RepositorioFacturacionElectronica();
            rta = Datos.InsertarClienteInterfaz(cliente);
            if (!rta.Equals("ERROR"))
            {
                return rta.ToString();
            }
            else
            {
                rta = "ERROR";
            }

            return rta.ToString();


        }

        public static string ActualizaEstadoCliente(Cliente cliente)
        {
            string rta = "";
            RepositorioFacturacionElectronica Datos = new RepositorioFacturacionElectronica();
            rta = Datos.ActualizaEstadoCliente(cliente);
            if (rta.Equals("OK"))
            {
                return rta.ToString();
            }
            else
            {
                rta = "ERROR";
            }
            return rta;
        }

        public static DataTable ValidarExisteCliente(Cliente cliente)
        {
            RepositorioFacturacionElectronica Datos = new RepositorioFacturacionElectronica();
            return Datos.ValidarExisteCliente(cliente);
        }
        #endregion



        #endregion

        #region Pagos 

        public static string InsetarPagos(string texto)
        {
            string rta = "";
            RepositorioFacturacionElectronica Datos = new RepositorioFacturacionElectronica();
            if (texto is null)
            {
                return rta = "No se encontro datos a insertar";
            }
            else
            {
                return Datos.InsertarPagos(texto);
            }
        }
        public static DataTable ConsultarDatosContablesEmpresas()
        {
            RepositorioFacturacionElectronica Datos = new RepositorioFacturacionElectronica();
            return Datos.ConsultarDatosContablesEmpresas();

        }

        //public static DataTable ListarDatosEmpresasPorEstacionamiento(int idEstacionamiento)
        //{

        //    RepositorioFacturacionElectronica Datos = new RepositorioFacturacionElectronica();
        //    return Datos.ListarDatosEmpresasPorEstacionamiento(idEstacionamiento);

        //}

        public static string InsertarCotizacionesEncabezado(string codigo)
        {
            RepositorioFacturacionElectronica Datos = new RepositorioFacturacionElectronica();
            return Datos.InsertarCotizacionesEncabezado(codigo);
        }

        public static DataTable ListarUltimaCotizacion()
        {
            RepositorioFacturacionElectronica Datos = new RepositorioFacturacionElectronica();
            return Datos.ListarUltimaCotizacion();
        }

        #region New 
        public static string InsetarPagosInterfaz(Cotizaciones cotizaciones)
        {
            string rta = "";
            RepositorioFacturacionElectronica Datos = new RepositorioFacturacionElectronica();
            rta = Datos.InsertarPagosInterfaz(cotizaciones);
            if (rta.Equals("OK"))
            {
                rta = "OK";
            }
            else
            {
                rta = "ERROR";
            }
            return rta;
        }

        public static string ActualizaEstadoPagos(Pagos pagos)
        {
            string rta = "";
            RepositorioFacturacionElectronica Datos = new RepositorioFacturacionElectronica();
            rta = Datos.ActualizaEstadoPagos(pagos);
            if (rta.Equals("OK"))
            {
                rta = "OK";
            }
            else
            {
                rta = "ERROR";
            }
            return rta;
        }

        public static string InsertaPagosCotizacionEncabezado(CotizacionEncabezado cotizacionEncabezado)
        {
            string rta = "";
            RepositorioFacturacionElectronica Datos = new RepositorioFacturacionElectronica();
            rta = Datos.InsertaPagosCotizacionEncabezado(cotizacionEncabezado);
            if (rta.Equals("OK"))
            {
                rta = "OK";
            }
            else
            {
                rta = "ERROR";
            }
            return rta;
        }


        #endregion

        #endregion

        //LISTADOS 

        public static DataTable ListarCotizacionesEncabezado()
        {
            RepositorioFacturacionElectronica Datos = new RepositorioFacturacionElectronica();
            return Datos.ListarCotizacionesEncabezado();
        }

        public static DataTable ListarCotizaciones()
        {
            RepositorioFacturacionElectronica Datos = new RepositorioFacturacionElectronica();
            return Datos.ListarCotizaciones();
        }

        public static DataTable ListarClientesInterfaz()
        {
            RepositorioFacturacionElectronica Datos = new RepositorioFacturacionElectronica();
            return Datos.ListarClientesInterfaz();
        }

        //public static DataTable ListarCentroCosto(int idEstacionamiento)
        //{
        //    RepositorioFacturacionElectronica Datos = new RepositorioFacturacionElectronica();
        //    return Datos.ListarCentroCosto(idEstacionamiento);
        //}

        public static DataTable ListarCentroCosto(Pagos pagos)
        {
            RepositorioFacturacionElectronica Datos = new RepositorioFacturacionElectronica();
            return Datos.ListarCentroCosto(pagos);
        }




        #region Contingencia

        public static string ListarFacturasContingencia()
        {
            string rta = string.Empty;
            RepositorioFacturacionElectronica Datos = new RepositorioFacturacionElectronica();
            DataTable tabla = new DataTable();
            Cotizaciones cotizaciones = new Cotizaciones();
            FacturasContingencia facturasContingencia = new FacturasContingencia();
            CotizacionEncabezado cotizacionEncabezado = new CotizacionEncabezado();
            int numeroFacturaAnterior = 1;
            int cotNum = 1;
            int itemCounter = 1;
            try
            {
                tabla = Datos.ListarFacturasContingencia();
                if (tabla.Rows.Count > 0)
                {
                    cotizaciones.Cot_Item = 1;

                    foreach (DataRow lstDatos in tabla.Rows)
                    {
                        facturasContingencia.IdPago = Convert.ToInt32(lstDatos["IdPago"]);
                        facturasContingencia.Empresa = Convert.ToInt32(lstDatos["Empresa"]);
                        facturasContingencia.FechaPago = Convert.ToDateTime(lstDatos["FechaPago"]);
                        facturasContingencia.IdentificacionCliente = lstDatos["Identificacion"].ToString();
                        facturasContingencia.CodigoSucursal = Convert.ToInt32(lstDatos["CodigoSucursal"]);
                        facturasContingencia.Prefijo = lstDatos["Prefijo"].ToString();
                        facturasContingencia.NumeroFactura = Convert.ToInt32(lstDatos["NumeroFactura"]);
                        facturasContingencia.Total = Convert.ToInt32(lstDatos["Total"]);
                        facturasContingencia.IdEstacionamiento = Convert.ToInt32(lstDatos["IdEstacionamiento"]);
                        facturasContingencia.IdTipoPago = Convert.ToInt32(lstDatos["IdTipoPago"]);
                        facturasContingencia.Vendedor = Convert.ToInt32(lstDatos["Vendedor"]);
                        facturasContingencia.Observaciones = lstDatos["Observaciones"].ToString();

                        tabla = ListarCentroCostoContingencia(facturasContingencia);
                        if (tabla.Rows.Count > 0)
                        {
                            foreach (DataRow lstDatosC in tabla.Rows)
                            {
                                cotizaciones.Cot_Centro_Costo = Convert.ToString(lstDatosC["CentroCosto"]);

                            }
                        }

                        tabla = ListarCotizaciones();
                        if (tabla.Rows.Count > 0)
                        {
                            foreach (DataRow lstDatosCot in tabla.Rows)
                            {
                                cotizaciones.Cot_Numero = Convert.ToInt32(lstDatosCot["Cot_Numero"]);
                            }
                        }
                        cotizaciones.Cot_Cantidad = 1;
                        cotizaciones.Cot_Valor_Unitario = Convert.ToInt32(facturasContingencia.Total);
                        cotizaciones.Cot_Documento = ConfigurationManager.AppSettings["PrefijoFE"];
                        cotizaciones.Cot_Tipo_Item = 2;
                        cotizaciones.Cot_Empresa = facturasContingencia.Empresa;

                        if (numeroFacturaAnterior != facturasContingencia.NumeroFactura)
                        {
                            cotNum = cotizaciones.Cot_Numero + 1;
                        }
                        else
                        {
                            cotNum = cotizaciones.Cot_Numero;
                        }
                        cotizaciones.Cot_Numero = cotNum;

                        if (facturasContingencia.IdTipoPago == 1)
                        {
                            cotizaciones.Cot_Referencia = "05";
                        }
                        else if (facturasContingencia.IdTipoPago == 2)
                        {
                            cotizaciones.Cot_Referencia = "06";
                        }
                        else if (facturasContingencia.IdTipoPago == 3)
                        {
                            cotizaciones.Cot_Referencia = "30";
                        }
                        else if (facturasContingencia.IdTipoPago == 4)
                        {
                            cotizaciones.Cot_Referencia = "31";
                        }
                        else if (facturasContingencia.IdTipoPago == 5)
                        {
                            cotizaciones.Cot_Referencia = "41";
                        }
                        else if (facturasContingencia.IdTipoPago == 6)
                        {
                            cotizaciones.Cot_Referencia = "98";
                        }

                        numeroFacturaAnterior = facturasContingencia.NumeroFactura;

                        rta = Datos.InsertarPagosInterfaz(cotizaciones);

                        if (rta.Equals("OK"))
                        {
                            cotizaciones.Cot_Item++;
                            rta = Datos.ActualizaEstadoPagosContingencia(facturasContingencia);
                            if (rta.Equals("OK"))
                            {
                                rta = "OK";
                            }
                            else
                            {
                                return rta;
                            }

                        }
                        else
                        {
                            return rta;
                        }



                    }
                    //Cotizaciones Encabezado

                    DateTime fechaHoy = DateTime.Now;
                    string fechaStr = fechaHoy.ToString("yyyy-MM-dd");

                    DateTime fechaNum = DateTime.ParseExact(fechaStr, "yyyy-MM-dd", null);

                    cotizacionEncabezado.Coe_Fecha = (int)(fechaHoy - new DateTime(1899, 12, 30)).TotalDays;
                    cotizacionEncabezado.Coe_Observaciones = "Reemplazo Factura de contingencia No  "+facturasContingencia.Observaciones+"";
                    cotizacionEncabezado.Coe_Empresa = cotizaciones.Cot_Empresa;
                    cotizacionEncabezado.Coe_Documento = ConfigurationManager.AppSettings["PrefijoFE"];
                    cotizacionEncabezado.Coe_Numero = cotizaciones.Cot_Numero;
                    cotizacionEncabezado.Coe_Cliente = Convert.ToInt32(facturasContingencia.IdentificacionCliente);
                    cotizacionEncabezado.Coe_Cliente_Sucursal = 1;
                    cotizacionEncabezado.Coe_Sincronizado = 0;
                    cotizacionEncabezado.Coe_Forma_Pago = 1;
                    tabla = Datos.ListarDocumentoVendedor();
                    if (tabla.Rows.Count > 0)
                    {
                        foreach (DataRow lstDcoVendedor in tabla.Rows)
                        {
                            cotizacionEncabezado.Coe_Vendedor = Convert.ToInt32(lstDcoVendedor["VEN_IDENTIFICACION"]);
                        }
                    }

                    rta = InsertaPagosCotizacionEncabezado(cotizacionEncabezado);
                    if (rta.Equals("OK"))
                    {
                        rta = "OK";
                    }
                    else
                    {
                        return rta;
                    }


                }

            }
            catch (Exception ex)
            {

                rta = ex.Message;
            }
            return rta;
        }

        public static DataTable ListarCentroCostoContingencia(FacturasContingencia facturasContingencia)
        {
            RepositorioFacturacionElectronica Datos = new RepositorioFacturacionElectronica();
            return Datos.ListarCentroCostoContingencia(facturasContingencia);
        }


        #endregion


    }
}
