namespace FacturacionElectronica.Models
{
    public class CotizacionesEncabezado : Estacionamientos
    {
        public int Empresa { get; set; } //1
        public string Documento { get; set; } //1
        public string Numero { get; set; }  //1
        public DateTime Fecha { get; set; }
        public int IdentificacionCliente { get; set; }
        public int ClienteSucursal { get; set; } //1
        public int NumeroMg { get; set; } //1
        public int Anticipo { get; set; } //0
        public int NumeroFactura { get; set; }
        public string Concepto { get; set; } //null
        public int Vendedor { get; set; } //0
        public string Observaciones { get; set; } //null
        public string Errores { get; set; } //null
        
        public bool Sincronizacion = false;
    }
}
