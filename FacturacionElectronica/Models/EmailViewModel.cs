using System.ComponentModel.DataAnnotations;

namespace FacturacionElectronica.Models
{
    public class EmailViewModel
    {
        [Required(ErrorMessage = "El nombre es requerido")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El correo es requerido")]
        [EmailAddress(ErrorMessage = "El dato ingresado no pertenece a un Email valido")]
        public string Email { get; set; }
        [Required(ErrorMessage = "El campo es requerido")]
        public IFormFile Attachment { get; set; }


    }
}
