using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public class Cotizaciones
    {
        public int Cot_Empresa { get; set; }
        public string Cot_Documento { get; set; }
        public int Cot_Numero { get; set; }
        public int Cot_Item { get; set; }
        public int Cot_Tipo_Item { get; set; }
        public string Cot_Descripcion_Item { get; set; }
        public string Cot_Referencia { get; set; }
        public int Cot_Bodega { get; set; }
        public int Cot_Cantidad { get; set; }
        public int Cot_Valor_Unitario { get; set; }
        public int Cot_Vr_Dto { get; set; }
        public int Cot_Centro_Costo { get; set; }
    }
}

