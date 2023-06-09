namespace FacturacionElectronica.Models
{
    public class Facturacion
    {
        public int IdFacturacion { get; set; }
        public string IdModulo { get; set; }
        public int IdEstacionamiento { get; set; }
        public string Prefijo { get; set; }
        public bool Estado { get; set; }
    }
}
