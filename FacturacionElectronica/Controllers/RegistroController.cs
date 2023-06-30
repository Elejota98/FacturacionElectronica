using FacturacionElectronica.Models;
using FacturacionElectronica.Servicios;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;

namespace FacturacionElectronica.Controllers
{
    public class RegistroController: Controller 
    {
        private readonly IRepositorioPagos repositorioPagos;

        public RegistroController( IRepositorioPagos repositorioPagos)
        {
            this.repositorioPagos = repositorioPagos;
        }
        public async Task<IActionResult> Index()
        {
            DateTime fechaInicio = DateTime.Now;
            DateTime fechaFin = DateTime.Now;

            string fechaInicioFinal = fechaInicio.Year + "-" + fechaInicio.Month + "-" + fechaInicio.Day; 
            string fechaFinalFinal = fechaFin.Year + "-"+fechaInicio.Month+"-"+fechaFin.Day;

            var reporteFacturas = await repositorioPagos.ListarFacturas(Convert.ToString(fechaInicioFinal), Convert.ToString(fechaFinalFinal));
            return View(reporteFacturas);

        }


    }
}
