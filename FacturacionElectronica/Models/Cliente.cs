using System.ComponentModel.DataAnnotations;

namespace FacturacionElectronica.Models
{
    public class Cliente
    {
        [Required(ErrorMessage ="El campo es requerido")]
        [Display(Name ="Identificación")]
        public string Identificacion { get; set; }
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
        public string Telefono { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        [EmailAddress(ErrorMessage ="El dato ingresado no es válido")]
        public string Email { get; set; }
        [Required(ErrorMessage ="El campo es requerido")]
        [Display(Name ="Departamento")]
        public string IdDepartamento { get; set; }
        [Range(0, maximum: int.MaxValue, ErrorMessage = "Debe seleccionar una ciudad")]
        [Display(Name = "Ciudad")]
        public int IdCiudad { get; set; }
        public string Ciudad { get; set; }
        public int Vendedor { get; set; }
        public int CupoCredito { get; set; }
        public DateTime Fecha { get; set; }
        public  bool Estado { get; set; }

    }
}
