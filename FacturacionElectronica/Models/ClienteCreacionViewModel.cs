using Microsoft.AspNetCore.Mvc.Rendering;

namespace FacturacionElectronica.Models
{
    public class ClienteCreacionViewModel : Cliente
    {
        public IEnumerable<SelectListItem> Departamentos { get; set; }
        public IEnumerable<SelectListItem> Ciudades { get; set; }
        public IEnumerable<SelectListItem> ActividadesEconomicas { get; set; }
    }
}
