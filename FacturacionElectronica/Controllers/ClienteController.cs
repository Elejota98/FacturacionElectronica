    using FacturacionElectronica.Models;
using FacturacionElectronica.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using System.Text;
using System.IO;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using AutoMapper;

namespace FacturacionElectronica.Controllers
{
    public class ClienteController: Controller
    {
        private readonly IRepositorioCliente repositorioCliente;
        private readonly IRepositorioParametros repositorioParametros;
        private readonly IMapper mapper;

        public ClienteController(IRepositorioCliente repositorioCliente, IRepositorioParametros repositorioParametros)
        {
            this.repositorioCliente = repositorioCliente;
            this.repositorioParametros = repositorioParametros;
        }
        [HttpGet]

        public async Task<IActionResult> Crear()
        {
            var modelo = new  ClienteCreacionViewModel();
             modelo.Departamentos = await ListarDepartamentos();
            modelo.ActividadesEconomicas = await ListarActividadEconomica();      
            return View(modelo);
        }


        public async Task<IEnumerable<SelectListItem>> ListarDepartamentos()
        {
            var departamentos = await repositorioCliente.ListarDepartamentos();
            var resultado = departamentos.Select(x => new SelectListItem(x.Nombre, x.Id.ToString())).ToList();
            var estacionamientoPorDefecto = new SelectListItem("Departamento...", "0", true);
            resultado.Insert(0, estacionamientoPorDefecto);
            return resultado;
        }

        public async Task<IEnumerable<SelectListItem>> ListarActividadEconomica()
        {
            var actividades = await repositorioCliente.ListarActividadEconomica();
            var rta = actividades.Select(x => new SelectListItem($"{x.Codigo} - {x.Descripcion}", x.Codigo.ToString())).ToList();
            var actividadesDefault = new SelectListItem("Actividades...", "0", true);
            rta.Insert(0, actividadesDefault); 
            return rta;
        }

        [HttpPost]

        public async Task<IActionResult> Crear(ClienteCreacionViewModel cliente, IFormFile Rut)
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
                    cliente.Rut = stream.ToArray();
                }
            }

            if (cliente.IdCiudad ==0 )
            {
                ModelState.AddModelError(nameof(cliente.IdCiudad), $"Por favor seleccione una ciudad.");
                return View(cliente);
            }
            var existeCliente = await repositorioCliente.Existe(cliente.Identificacion);

            if (existeCliente)
            {
                ModelState.AddModelError(nameof(cliente.Identificacion), $"Este documento {cliente.Identificacion} ya se encuentra registrado.");
                return RedirectToAction("Crear", "Pagos");
            }
            await repositorioCliente.Crear(cliente);
            return RedirectToAction("Crear", "Pagos");
        }

        public async Task<IActionResult> Index()
        {
            var clientes = await repositorioCliente.ObtenerClientes();
            return View(clientes);
        }
        private async Task<IEnumerable<SelectListItem>> ObtenerCiudadesPorDepartamento(int idDepartamento)
        {
            var ciudades = await repositorioCliente.ListarCiudadesPorDepartamento(idDepartamento);
            var rtaCiudades = ciudades.Select(x=> new SelectListItem(x.Nombre, x.Id.ToString())).ToList();
            var opcionPorDefecto = new SelectListItem("Ciudad...", "0", true);
            rtaCiudades.Insert(0, opcionPorDefecto);

            return rtaCiudades;
        }


        [HttpPost]
        public async Task<IActionResult> ListarCiudades([FromBody] int idDepartamento)
        {
            var ciudades = await ObtenerCiudadesPorDepartamento(idDepartamento);
            return Ok(ciudades);
        }

    }
}
