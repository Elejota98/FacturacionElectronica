using Modelo;
using Servicios;
using System;
using System.Collections.Generic;
using System.Data;
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
            RepositorioFacturacionElectronica Datos = new  RepositorioFacturacionElectronica();
            return Datos.ActualizaClientes();
        }

        public static DataTable ListarPagos()
        {
            RepositorioFacturacionElectronica Datos = new RepositorioFacturacionElectronica();
            return Datos.ListarPagos();
        }

        public static string ActualizaEstadoPagos(int id)
        {
            RepositorioFacturacionElectronica Datos = new RepositorioFacturacionElectronica();
            return Datos.ActualizaEstadoPagos(id);
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
            if(texto is null)
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
            RepositorioFacturacionElectronica Datos = new  RepositorioFacturacionElectronica();
            return Datos.ListarUltimaCotizacion();
        }

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

        public static DataTable ListarCentroCosto(int idEstacionamiento)
        {
            RepositorioFacturacionElectronica Datos = new RepositorioFacturacionElectronica();
            return Datos.ListarCentroCosto(idEstacionamiento);
        }


    }
}
