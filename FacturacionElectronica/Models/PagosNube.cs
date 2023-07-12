namespace FacturacionElectronica.Models
{
    public class PagosNube
    {
        public long IdPago { get; set; }
        public string IdTransaccion { get; set; }
        public long IdAutorizado { get; set; }
        public long IdEstacionamiento { get; set; }
        public string IdModulo { get; set; }
        public long IdFacturacion { get; set; }
        public long IdTipoPago { get; set; }
        public DateTime FechaPago { get; set; }
        public double Subtotal { get; set; }
        public double Iva { get; set; }
        public double Total { get; set; }
        public int NumeroFactura { get; set; }
        public bool Sincronizacion { get; set; }
        public bool PagoMensual { get; set; }
        public bool Anulada { get; set; }
    }
}
