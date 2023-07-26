using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Security.Principal;

namespace FacturacionElectronica.Models
{
    public class Pagos : TipoPagos
    {
        public int IdPago { get; set; }
        [Required(ErrorMessage ="El número de documento es obligatorio")]
        [Display(Name ="Identificación")]
       public string Identificacion { get; set; }
        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Número de factura")]
        public int NumeroFactura { get; set; }
        public string Prefijo { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public int Total { get; set; }

        [Required(ErrorMessage = "Este campo es obligatorio")]
        [Display(Name = "Sede - Parqueadero")]
        public int IdEstacionamiento { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public int IdTipoPago { get; set; }
        public byte[] Imagen { get; set; }

        public bool Estado = false;
        [Required(ErrorMessage = "Este campo es requerido")]
        [Display(Name = "Fecha de la factura")]
        [Remote(action: "ValidarDiasFechaFactura", controller:"Pagos")]
        public DateTime FechaPago { get; set; }
    }
}
