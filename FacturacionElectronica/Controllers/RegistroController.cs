using FacturacionElectronica.Models;
using FacturacionElectronica.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace FacturacionElectronica.Controllers
{
    public class RegistroController: Controller 
    {
        private readonly IRepositorioCotizaciones repositorioCotizaciones;

        public RegistroController( IRepositorioCotizaciones repositorioCotizaciones)
        {
            this.repositorioCotizaciones = repositorioCotizaciones;
        }

        public async Task<IActionResult> Crear()
        {
            //var modelo = new CotizacionEncabezadoCreacionViewModel();
            //modelo.Estacionamientos = await repositorioCotizaciones.ListarEstacionamientos();
            return View();
        }

        [HttpPost]

        public async Task<IActionResult> Crear(CotizacionesEncabezado cotizacionesEncabezado)
        {
            //Validar si existe el cliente 

            var existe = await repositorioCotizaciones.ClienteExiste(cotizacionesEncabezado.IdentificacionCliente);
            if (!existe)
            {
                return RedirectToAction("Cliente", "Crear");
            }
            return View(cotizacionesEncabezado);
        }
    }
}
