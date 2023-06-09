using FacturacionElectronica.Controllers;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace FacturacionElectronica.Models
{
    public class CotizacionEncabezadoCreacionViewModel : CotizacionesEncabezado
    {
        public IEnumerable<SelectListItem> Estacionamientos { get; set; }

    }
}
