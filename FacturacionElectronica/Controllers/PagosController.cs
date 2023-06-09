﻿using FacturacionElectronica.Models;
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

        public async Task<IActionResult> Index()
        {
            return View();
        }

        public async Task<IActionResult> Crear()
        {
            var modelo = new PagosCreacionViewModel();
            modelo.Estacionamientos = await ListarEstacionamientos();
            modelo.FechaPago = DateTime.Now;
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
        public async Task<IActionResult> VerificarExisteCliente([FromQuery] string identificacion)
        {
            var yaExisteCliente = await repositorioPagos.Existe(identificacion);
            if (!yaExisteCliente)
            {
                //return Json($"El documento {pagos.NumeroDocumento}, no se encuentra registrado, por favor registrelo en el botón registar cliente");
                return RedirectToAction("Crear", "Cliente");
            }
            return Json(identificacion);
        }

        //Crear el registro pagos 
        [HttpPost]
        public async Task<IActionResult> Crear(PagosCreacionViewModel pagos, IFormFile imagen)
        {
            Pagos oPagos = new Pagos();
            PagosNube oPagosNube = new PagosNube();
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
            if (pagos.Total == 0)
            {
                return RedirectToAction("NoExiste", "Home");
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
            //var datosFactura = await repositorioPagos.ListarPagosNube(pagos.NumeroFactura, pagos.IdEstacionamiento,  idModulo.IdModulo);
            //if (datosFactura is null)
            //{
            //    return RedirectToAction("NoEncontrado", "Home");
            //}
            //else if (datosFactura != null)
            //{
            //    DateTime fechaPagosNube;
            //    fechaPagosNube = datosFactura.FechaPago;
            //    if (pagos.FechaPago.Day != fechaPagosNube.Day)
            //    {
            //        return RedirectToAction("NoEncontrado", "Home");
            //    }

            //}

            //VALIDAR SI EXISTE LA FACTURA

            var existeFactura = await repositorioPagos.ExisteFactura(pagos.IdEstacionamiento, idModulo.IdModulo, pagos.FechaPago, pagos.NumeroFactura);
            if (existeFactura)
            {

                var datosFactura = await repositorioPagos.ListarPagosNube(pagos.NumeroFactura, pagos.IdEstacionamiento, idModulo.IdModulo);

                if (datosFactura is null)
                {
                    return RedirectToAction("NoEncontrado", "Home");
                }

                bool facturaEncontrada = false;
                foreach (var factura in datosFactura)
                {
                    if (factura.NumeroFactura == pagos.NumeroFactura)
                    {
                        facturaEncontrada = true;
                        DateTime fechaPagosNube = DateTime.Now;

                        if (pagos.FechaPago.Day != fechaPagosNube.Day)
                        {
                            DateTime fechaActual = DateTime.Now;
                            TimeSpan diferencia = fechaActual.Date - pagos.FechaPago.Date;
                            int diasDiferencia = diferencia.Days;

                            if (diasDiferencia > 3)
                            {
                                return RedirectToAction("NoEncontrado", "Home");
                            }
                        }
                    }
                }

                if (!facturaEncontrada)
                {
                    return RedirectToAction("NoEncontrado", "Home");
                }

                var listadoPagos = await repositorioPagos.ListarTotalesSeparados(pagos.NumeroFactura, idModulo.IdModulo, pagos.IdEstacionamiento);
                foreach (var pagoslist in listadoPagos)
                {
                    pagos.NumeroFactura = pagoslist.NumeroFactura;
                    pagos.Total = pagoslist.Total;
                    pagos.IdPago = pagoslist.IdPago;
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

                    var listadoPagosNube = await repositorioPagos.ListarPagos(pagoslist.IdPago);
                    foreach (var pagosNubeList in listadoPagosNube)
                    {
                        pagosNubeList.IdTransaccion = pagos.Identificacion;
                        if (pagosNubeList.IdTipoPago == 2 && pagosNubeList.IdAutorizado > 0)
                        {
                            var listarTipoVehiculo = await repositorioPagos.ListarTipoVehiculo(pagosNubeList.IdAutorizado, pagosNubeList.IdTipoPago, pagosNubeList.IdEstacionamiento);

                            foreach (var listTipoVehiculo in listarTipoVehiculo)
                            {
                                if (listTipoVehiculo.IdTipoVehiculo == 1)
                                {
                                    pagosNubeList.IdTipoPago = 1;
                                }
                                else
                                {
                                    pagosNubeList.IdTipoPago = 2;
                                }


                            }

                        }
                        pagosNubeList.IdTipoPago = pagos.IdTipoPago;

                        await repositorioPagos.InsertarPagosFE(pagosNubeList);
                    }

                    if (pagos.IdPago < 0)
                    {
                        RedirectToAction("NoEncontrado", "Home");
                    }

                }
            }
            else
            {
                return RedirectToAction("NoExiste", "Home");
            }
            return RedirectToAction("Index", "Pagos");
        }

    }
}
