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
        [HttpPost]
        public async Task<IActionResult> VerificarExisteCliente([FromQuery] string numeroDocumento)
        {
            var yaExisteCliente = await repositorioPagos.Existe(numeroDocumento);
            if (!yaExisteCliente)
            {
                //return Json($"El documento {pagos.NumeroDocumento}, no se encuentra registrado, por favor registrelo en el botón registar cliente");
                return RedirectToAction("Crear", "Cliente");
            }
            return Json(numeroDocumento);
        }

        //Crear el registro pagos 
        [HttpPost]
        public async Task<IActionResult> Crear(PagosCreacionViewModel pagos, IFormFile imagen)
        {
            Pagos oPagos = new Pagos();
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

            var idModulo = await repositorioPagos.ListarIdModuloPorPrefijo(pagos.Prefijo,pagos.IdEstacionamiento);
            if (idModulo is null)
            {
                 return RedirectToAction("NoEncontrado", "Home");
            }

            var idTipoPago = await repositorioPagos.ListarTipoPago(pagos.TipoPago);
            if (idTipoPago is null)
            {
                return RedirectToAction("NoEncontrado", "Home");
            }
            

            var listadoPagos = await repositorioPagos.ListarTotalesSeparados(pagos.NumeroFactura, idModulo.IdModulo, pagos.IdEstacionamiento);
            foreach (var pagoslist in listadoPagos)
            {
                pagos.NumeroFactura = pagoslist.NumeroFactura;
                pagos.Total= pagoslist.Total;
                if (pagoslist.IdTipoPago == 6)
                {
                    pagos.IdTipoPago = 5;
                }
                else if (pagoslist.IdTipoPago == 3)
                {
                    pagos.IdTipoPago = 6;
                }
                else
                {
                    pagos.IdTipoPago = idTipoPago.IdTipoPago;
                }


                await repositorioPagos.Insertar(pagos);
                if (pagos.Id < 0)
                {
                    RedirectToAction("NoEncontrado", "Home");
                }

            }

            //await GenerarScriptsPagos();
            return RedirectToAction("Crear");
        }

    }
}
