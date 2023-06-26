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

        //INTERFAZ


        #region Cliente
        public static string InsertarClienteInterfaz(string texto)
        {
            RepositorioFacturacionElectronica Datos = new RepositorioFacturacionElectronica();
            return Datos.InsertarClienteInterfaz(texto);
        }
        public static DataTable ValidarExisteCliente( string documento)
        {
            RepositorioFacturacionElectronica Datos = new RepositorioFacturacionElectronica();
            return Datos.ValidarExisteCliente(documento);
        }

        public static DataTable ListarDocumentoVendedor()
        {
            RepositorioFacturacionElectronica Datos = new RepositorioFacturacionElectronica();
            return Datos.ListarDocumentoVendedor();
        }



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

        public static DataTable ListarDatosEmpresasPorEstacionamiento(int idEstacionamiento)
        {

            RepositorioFacturacionElectronica Datos = new RepositorioFacturacionElectronica();
            return Datos.ListarDatosEmpresasPorEstacionamiento(idEstacionamiento);

        }

        public static string InsertarCotizacionesEncabezado(string codigo)
        {
            RepositorioFacturacionElectronica Datos = new RepositorioFacturacionElectronica();
            return Datos.InsertarCotizacionesEncabezado(codigo);
        }
        #endregion


    }
}
