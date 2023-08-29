using Controlador;
using Modelo;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace FacturacionElectronicaServicio
{
    public partial class SincronizacionFacturacionElectronica : Form
    {
        #region Definiciones
        Cliente cliente = new Cliente();

        #endregion
        public SincronizacionFacturacionElectronica()
        {
            InitializeComponent();
        }

        #region Funciones
        public bool ListarClientes()
        {
            bool ok = false;
            DataTable tabla = new DataTable();


            tabla = FacturacionElectronicaController.ListarClientes();
            if (tabla.Rows.Count > 0)
            {
                foreach (DataRow row in tabla.Rows)
                {
                    
                    cliente.Identificacion= Convert.ToInt32(row["Identificacion"].ToString());
                    cliente.RazonSocial = row["RazonSocial"].ToString();
                    cliente.Direccion = row["Direccion"].ToString();
                    cliente.Telefono = row["Telefono"].ToString();
                    cliente.Email = row["Email"].ToString();
                    cliente.Nombre = row["Nombre"].ToString();
                    cliente.Fecha = Convert.ToDateTime(row["Fecha"]);
                    cliente.FechaSeparada = cliente.Fecha.ToString("dd.MM.yyyy HH.mm");
                    cliente.Estado = Convert.ToBoolean(row["Estado"]);
                    cliente.Vendedor = ListarDocumentoVendedor();
                    cliente.Empresa = 1;
                }
                ok = true;
            }
            return ok;
        }

        public int ListarDocumentoVendedor()
        {
            DataTable tabla = new DataTable();
            int documento = 0;
            try
            {
                tabla = FacturacionElectronicaController.ListarDocumentoVendedor();
                foreach (DataRow rtaTabla in tabla.Rows)
                {
                     documento = Convert.ToInt32(rtaTabla["VEN_IDENTIFICACION"]);

                }
            }
            catch (Exception ex )
            {

                throw ex ;
            }
            return documento;
        }

        #endregion

        #region Mensajes

        #endregion
    }
}
