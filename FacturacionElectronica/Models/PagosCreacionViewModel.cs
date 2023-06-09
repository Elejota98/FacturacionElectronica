using Microsoft.AspNetCore.Mvc.Rendering;

namespace FacturacionElectronica.Models
{
    public class PagosCreacionViewModel : Pagos
    {
        public IEnumerable<SelectListItem> Estacionamientos { get; set; }
        public IEnumerable<SelectListItem> Prefijos { get; set; }

    }
}
