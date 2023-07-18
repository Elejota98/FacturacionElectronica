using FacturacionElectronica.Models;
using FacturacionElectronica.Servicios;
using Microsoft.AspNetCore.Mvc;
using static System.Net.Mime.MediaTypeNames;

namespace FacturacionElectronica.Controllers
{

    public class SolicitudClienteController : Controller
    {
        private readonly IRepositorioCliente repositorioCliente;

        public SolicitudClienteController(IRepositorioCliente repositorioCliente)
        {
            this.repositorioCliente = repositorioCliente;
        }

        public async Task<IActionResult> Index()
        {
            return View();
        }
        public async Task<IActionResult> Crear()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Crear(SolicitudClientesViewModel clientes, IFormFile Rut)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            if (Rut != null && Rut.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    Rut.CopyTo(stream);
                   clientes.Rut = stream.ToArray();
                }
            }
            await repositorioCliente.CrearSolicitudCliente(clientes);
            return View();


        }

    }
}
