using Servicios;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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
    }
}
