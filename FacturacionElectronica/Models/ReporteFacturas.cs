namespace FacturacionElectronica.Models
{
    public class ReporteFacturas
    {
        public int Id { get; set; }
        public int NumeroDocumento { get; set; }
        public int NumeroFactura { get; set; }
        public string Prefijo { get; set; }
        public int Total { get; set; }
        public string Nombre { get; set; }
        public string TipoPago { get; set; }
        public DateTime FechaPago { get; set; }
        public byte[] Imagen { get; set; }
        public bool Estado { get; set; }
    }
}
