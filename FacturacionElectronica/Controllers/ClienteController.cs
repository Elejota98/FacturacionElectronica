    using FacturacionElectronica.Models;
using FacturacionElectronica.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using System.Text;
using System.IO;

namespace FacturacionElectronica.Controllers
{
    public class ClienteController: Controller
    {
        private readonly IRepositorioCliente repositorioCliente;
        private readonly IRepositorioParametros repositorioParametros;

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

       [HttpPost]

        public async Task<IActionResult> Crear(ClienteCreacionViewModel cliente)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }

            if(cliente.IdCiudad ==0 )
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
            //await GenerarScripts();
            return RedirectToAction("Crear", "Pagos");
        }

        public async Task<IActionResult> Index()
        {
            var clientes = await repositorioCliente.ObtenerClientes();
            return View(clientes);
        }

        //GENERAR ARCHIVO PLANO

        [HttpPost]
        public async Task<IActionResult> GenerarScripts()
        {
            var registros = await repositorioCliente.ObtenerClientes();
            var scripts = new StringBuilder();

            foreach (var registrosClientes in registros)
            {
                scripts.AppendLine($"INSERT INTO CLIENTES (CLI_EMPRESA, CLI_IDENTIFICACION, CLI_CODIGO_SUCURSAL, CLI_RAZON_SOCIAL, CLI_DIRECCION, CLI_TELEFONO, " +
                    $"CLI_EMAIL_FE, CLI_CIUDAD, CLI_VENDEDOR, CLI_CUPO_CREDITO, CLI_FECHA_UPDATE) " +
                    $"VALUES ('1', '{registrosClientes.Identificacion}', 0, '{registrosClientes.RazonSocial}', '{registrosClientes.Direccion}', " +
                    $"'{registrosClientes.Telefono}', '{registrosClientes.Email}', '{registrosClientes.IdCiudad}', '1', 0, '{registrosClientes.Fecha}')");
            }

            var parametros = await repositorioParametros.ListarRuta();
            string rutaArchivo = "";
            for (int i = 0; i < parametros.Count; i++)
            {
                if (parametros[i].Codigo == "RutaArchivoCliente")
                {
                    rutaArchivo = parametros[i].Valor;
                }
            }

            if (!Directory.Exists(rutaArchivo))
            {
                Directory.CreateDirectory(rutaArchivo);
            }

            string rutaArchivoN = Path.Combine(rutaArchivo, "InterfazCliente.txt");
            using (StreamWriter writer = new StreamWriter(rutaArchivoN, true))
            {
                await writer.WriteAsync(scripts.ToString());
            }

            await repositorioCliente.ActualizarEstado();

            return View();
        }

        //Consultar Ciudades por departamneto

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
