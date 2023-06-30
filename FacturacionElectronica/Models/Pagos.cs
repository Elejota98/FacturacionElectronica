﻿using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace FacturacionElectronica.Models
{
    public class Pagos : TipoPagos
    {
        public int Id { get; set; }
        [Required(ErrorMessage ="El número de documento es obligatorio")]
        [Display(Name ="Identificación")]
        public string NumeroDocumento { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Número de factura")]
        public int NumeroFactura { get; set; }
        public string Prefijo { get; set; }
        public int Total { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Sede - Parqueadero")]
        public int IdEstacionamiento { get; set; }
        public int IdTipoPago { get; set; }
        public byte[] Imagen { get; set; }

        public bool Estado = false;
        [Required(ErrorMessage ="Este campo es requerido")]
        [Display(Name ="Fecha de la factura")]
        public DateTime FechaPago = DateTime.Now;
    }
}
