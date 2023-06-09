namespace FacturacionElectronica.Models
{
    public class Cotizaciones
    {
        public int Empresa { get; set; }
        public DateTime Fecha { get; set; }
        public int Identificacion { get; set; }
        public int CodigoSucursal { get; set; }
        public string Prefijo { get; set; }
        public int NumeroFactura { get; set; }
        public int Vendedor { get; set; }

    }
}
