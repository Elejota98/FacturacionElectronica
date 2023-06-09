using FacturacionElectronica.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace FacturacionElectronica.Controllers
{
    public class CotizacionController : Controller
    {
        private readonly IRepositorioCotizaciones repositorioCotizaciones;

        public CotizacionController(IRepositorioCotizaciones repositorioCotizaciones)
        {
            this.repositorioCotizaciones = repositorioCotizaciones;
        }

        public async Task<IActionResult> Crear()
        {
            return View();
        }
    }
}
