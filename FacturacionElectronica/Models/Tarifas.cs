namespace FacturacionElectronica.Models
{
    public class Tarifas
    {
        public long IdTarifa { get; set; }
        public long IdEstacionamiento { get; set; }
        public long IdTipoPago { get; set; }
        public long IdTipoVehiculo { get; set; }
        public long IdConvenio { get; set; }
        public long IdAutorizado { get; set; }
        public long IdEvento { get; set; }
        public double Valor { get; set; }
        public string TipoCobro { get; set; }
        public bool Estado { get; set; }
        public bool Sincronizacion { get; set; }
    }
}
