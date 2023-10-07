using System.ComponentModel.DataAnnotations;

namespace FacturacionElectronica.Models
{
    public class Cliente
    {
        [Required(ErrorMessage ="El campo es requerido")]
        [Display(Name ="Identificación")]
        public int Identificacion { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        [Display(Name = "Tipo de persona")]

        public string TipoPersona { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        [Display(Name = "Tipo de documento")]
        public string TipoDocumento { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        [Display(Name = "Nombres")]
        public string NombreApellidos { get; set; }

        public int Empresa { get; set; }
        [Required(ErrorMessage ="El campo es requerido")]
        [Display(Name ="Razón Social")]
        public string RazonSocial { get; set; }
        [Required(ErrorMessage ="El campo es requerido")]
        [Display(Name ="Dirección")]
        public string Direccion { get; set; }
        [Display(Name ="Teléfono")]
        public string Telefono { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        [EmailAddress(ErrorMessage ="El dato ingresado no es válido")]
        [Display(Name ="Correo - (Facturación Electrónica)")]
        public string Email { get; set; }
        [Range(0, maximum: int.MaxValue, ErrorMessage = "Debe seleccionar un departamento")]
        [Required(ErrorMessage ="El campo es requerido")]
        [Display(Name ="Departamento")]
        public string IdDepartamento { get; set; }
        [Range(0, maximum: int.MaxValue, ErrorMessage = "Debe seleccionar una ciudad")]
        [Required(ErrorMessage = "El campo es requerido")]
        [Display(Name = "Ciudad")]
        public int IdCiudad { get; set; }
        public string Ciudad { get; set; }
        [Display(Name ="Actividad Económica")]
        public int ActividadEconomica { get; set; }
        [Display(Name ="Actividad Fiscal")]
        public string ResponsabilidadFiscal { get; set; }
        [Display(Name = "Régimen")]
        [Required(ErrorMessage = "El campo es requerido")]

        public string Regimen { get; set; }
        public byte[] Rut { get; set; }
        public int Vendedor { get; set; }
        public int CupoCredito { get; set; }
        public DateTime Fecha { get; set; }
        public  bool Estado { get; set; }

    }
}
