using Modelo;
using Servicios;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;

namespace Controlador
{
    public class NotaCreditoController
    {
        public static DataTable ListarEstacionamientos()
        {
            RepositorioNotaCredito Datos = new RepositorioNotaCredito();
            return Datos.ListarEstacionamientos();
        }

        public static DataTable ListarPagosInterfaz(int idEstacionamiento, string fechaPago, string fechaPagoFin)
        {
            RepositorioNotaCredito Datos = new RepositorioNotaCredito();
            return Datos.ListarPagosInterfaz(idEstacionamiento,fechaPago,fechaPagoFin);
        }

        public static DataTable ListarTodosPagosInterfaz()
        {
            RepositorioNotaCredito Datos = new RepositorioNotaCredito();
            return Datos.ListarTodosPagosInterfaz();
        }

        public static DataTable ListarInterfaz(int idEstacionamiento, string numeroFactura, string fechaPago)
        {
            RepositorioNotaCredito Datos = new RepositorioNotaCredito();
            return Datos.ListarInterfaz(idEstacionamiento, numeroFactura, fechaPago);
        }

        //INTERFAZ



        public static DataTable ListarItemsContable()
        {
            RepositorioNotaCredito Datos = new RepositorioNotaCredito();
            return Datos.ListarItemsContable();
        }
        public static DataTable ListarDocContable()
        {
            RepositorioNotaCredito Datos = new RepositorioNotaCredito();
            return Datos.ListarDocContable();
        }

        public static DataTable ListarDocumentoEmpresa(int idEstacionamiento)
        {
            RepositorioNotaCredito Datos = new RepositorioNotaCredito();
            return Datos.ListarDocumentoEmpresa(idEstacionamiento);
        }

        public static DataTable VerificarSiExisteElRegistro(DateTime fecha, int idcEmpresa, string idcDocumento, string numeroFactura)
        {
      
            RepositorioNotaCredito Datos = new RepositorioNotaCredito();
            return Datos.ValidarSiExisteElRegistro(fecha, idcEmpresa,idcDocumento,numeroFactura);
        }

        public static DataTable GenerarDatosASubir(int idEstacionamiento, DateTime fecha, int numeroFactura)
        {
            RepositorioNotaCredito Datos = new RepositorioNotaCredito();
            return Datos.GenerarDatosASubir(idEstacionamiento, fecha, numeroFactura);
        }
        public static bool InsertarItemsContable(DataTable datos, int itemConsecutivo, int idEstacionamiento,  string numeroFactura, string idModulo)
        {
            bool ok = false;
            RepositorioNotaCredito Datos = new RepositorioNotaCredito();
            if (Datos.InsertarItemsContable(datos, itemConsecutivo, idEstacionamiento,numeroFactura,idModulo))
            {
                ok = true;
            }
            else
            {
                return ok;
            }
            return ok;

        }
        public static DataTable ListarPagosAnular(int idEstacionamiento, DateTime fecha, int numeroFactura)
        {
            RepositorioNotaCredito Datos = new RepositorioNotaCredito();
            return Datos.ListarPagosParaAnular(idEstacionamiento,fecha,numeroFactura);
        }
        //public static string AnularFacturaPOS(int idPago)
        //{
        //    RepositorioNotaCredito Datos = new RepositorioNotaCredito();
        //    return Datos.AnularFacturaPOS(idPago, );
        //}



    }
}
