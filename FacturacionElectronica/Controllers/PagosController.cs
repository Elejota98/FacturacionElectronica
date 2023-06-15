using FacturacionElectronica.Models;
using FacturacionElectronica.Servicios;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc.TagHelpers;
using System.Text;

namespace FacturacionElectronica.Controllers
{
    public class PagosController : Controller
    {
        private readonly IRepositorioPagos repositorioPagos;
        private readonly IRepositorioParametros repositorioParametros;
        Estacionamientos estacionamientos = new Estacionamientos();

        public PagosController(IRepositorioPagos repositorioPagos, IRepositorioParametros repositorioParametros)
        {
            this.repositorioPagos = repositorioPagos;
            this.repositorioParametros = repositorioParametros;
        }

        public async Task<IActionResult> Crear()
        {
            var modelo = new PagosCreacionViewModel();
            modelo.Estacionamientos = await ListarEstacionamientos();
            return View(modelo);
        }


        //Listar Estacionamientos
        private async Task<IEnumerable<SelectListItem>> ListarEstacionamientos()
        {
            var estacionamientos = await repositorioPagos.ListarEstacionamientos();
            var resultado = estacionamientos.Select(x => new SelectListItem(x.Nombre, x.IdEstacionamiento.ToString())).ToList();
            var estacionamientoPorDefecto = new SelectListItem("Seleccione la sede...", "0", true);
            resultado.Insert(0, estacionamientoPorDefecto);
            return resultado;
        }

        //Listar Prefijos 

        [HttpPost]
        public async Task<IActionResult> ListarPrefijos([FromBody] int idEstacionamiento)
        {
            var prefijos = await ObtenerPrefijos(Convert.ToInt32(idEstacionamiento));
            return Ok(prefijos);
        }
        private async Task<IEnumerable<SelectListItem>> ObtenerPrefijos(int idEstacionamiento)
        {
            var prefijos = await repositorioPagos.ObtenerPrefijoPorIdEstacionamiento(idEstacionamiento);
            var resultadoPrefijos = prefijos.Select(x => new SelectListItem(x.Prefijo, x.Prefijo.ToString())).ToList();

            var opcionPorDefecto = new SelectListItem("Seleccione un prefijo", "0", true);

            resultadoPrefijos.Insert(0, opcionPorDefecto);

            return resultadoPrefijos;
        }

        //Listar total por numero de factura 
        [HttpPost]
        public async Task<IActionResult> ListarTotalPorParametros([FromQuery] int IdEstacionamiento, string Prefijo, string NumeroFactura)
        {

            var idModulo = await repositorioPagos.ListarIdModuloPorPrefijo(Prefijo, IdEstacionamiento);
            if (idModulo is null)
            {
                return RedirectToAction("Crear");
            }
            var total = await repositorioPagos.ListarTotal(Convert.ToInt32(NumeroFactura), idModulo.IdModulo, IdEstacionamiento);
            if(total.Total==0)
            {
                return RedirectToAction("Crear");
            }
            return Ok(total.Total);
        }

        //Listar el total 
        [HttpPost]
        public async Task<IActionResult> ListarTotal([FromBody] int idEstacionamiento)
        {
            estacionamientos.IdEstacionamiento = idEstacionamiento;
            var prefijos = await ObtenerPrefijos(Convert.ToInt32(estacionamientos.IdEstacionamiento));
            return Ok(prefijos);
        }

        //Validar si el cliente existe en la base de datos
        [HttpGet]
        public async Task<IActionResult> VerificarExisteCliente(Pagos pagos)
        {
            var yaExisteCliente = await repositorioPagos.Existe(pagos.NumeroDocumento);
            if (!yaExisteCliente)
            {
                //return Json($"El documento {pagos.NumeroDocumento}, no se encuentra registrado, por favor registrelo en el botón registar cliente");
                return RedirectToAction("Crear", "Cliente");
            }
            return Json(true);
        }

        //Crear el registro pagos 
        [HttpPost]
        public async Task<IActionResult> Crear(PagosCreacionViewModel pagos, IFormFile imagen)
        {
            //ObtenerEstacionamientosPorId
            var idEstacionamiento = await repositorioPagos.ListarEstacionamientos();
            if (idEstacionamiento is null)
            {
                RedirectToAction("NoEncontrado", "Home");
            }
            if (imagen != null && imagen.Length > 0)
            {
                using (var stream = new MemoryStream())
                {
                    imagen.CopyTo(stream);
                    pagos.Imagen = stream.ToArray();
                }
            }
            if (!ModelState.IsValid)
            {
                pagos.Estacionamientos = await ListarEstacionamientos();
                return View(pagos);
            }
            await repositorioPagos.Insertar(pagos);
            if (pagos.Id < 0)
            {
                RedirectToAction("NoEncontrado", "Home");
            }
            //await GenerarScriptsPagos();
            return RedirectToAction("Registros");
        }


        //[HttpPost]
        //public async Task<IActionResult> GenerarScriptsPagos()
        //{
        //    var registrosPagos = await repositorioPagos.ListarDatosPagos();
        //    var scripts = new StringBuilder();
        //    foreach (var registros in registrosPagos)
        //    {
        //        scripts.AppendLine($"INSERT INTO COTIZACIONES (COE_EMPRESA, COE_DOCUMENTO,COE_NUMERO,COE_FECHA,COE_CLIENTE,COE_CLIENTE_SUCURSAL,COE_SINCRONIZADO,COE_ERRORES,COE_OBSERVACIONES," +
        //                            $"COE_NUMERO_MG,COE_FECHA_UPDATE,COE_ANTICIPO,COE_FRA_PREFIJO,COE_FRA_NUMERO, COE_DEV_CONCEPTO,COE_VENDEDOR)" +
        //                            $"VALUES({registros.Empresa},{registros.Identificacion},1,{registros.Fecha},1,1,1,'NULL','NULL',1,'NULL',0,{registros.Prefijo},{registros.NumeroFactura},'NULL','NULL')");
        //    }
        //    var parametros = await repositorioParametros.ListarRuta();
        //    string rutaArchivo = "";
        //    for (int i = 0; i < parametros.Count; i++)
        //    {
        //        if (parametros[i].Codigo == "RutaArchivoCliente")
        //        {
        //            rutaArchivo = parametros[i].Valor;
        //        }
        //    }

        //    if (!Directory.Exists(rutaArchivo))
        //    {
        //        Directory.CreateDirectory(rutaArchivo);
        //    }

        //    string rutaArchivoN = Path.Combine(rutaArchivo, "InterfazCliente.txt");
        //    using (StreamWriter writer = new StreamWriter(rutaArchivoN, true))
        //    {
        //        await writer.WriteAsync(scripts.ToString());
        //    }

        //    await repositorioPagos.ActualizarEstadoPago();

        //    return View();

        //}


    }
}
