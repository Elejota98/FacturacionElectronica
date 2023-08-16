using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public class CotizacionEncabezado
    {
        public int Coe_Empresa { get; set; }
        public string Coe_Documento { get; set; }
        public string Coe_Numero { get; set; }
        public int Coe_Fecha { get; set; }
        public int Coe_Cliente { get; set; }
        public int Coe_Cliente_Sucursal { get; set; }
        public int Coe_Sincronizado { get; set; }
        public string Coe_Errores { get; set; }
        public string Coe_Observaciones { get; set; }
        public string Coe_Numero_Mg { get; set; }
        public DateTime Coe_Fecha_Update{ get; set; }
        public int Coe_Anticipo { get; set; }
        public string Coe_Fra_Prefijo { get; set; }
        public string Coe_Fra_Numero { get; set; }
        public int Coe_Dev_Concepto { get; set; }
        public int Coe_Vendedor { get; set; }

    }
}
