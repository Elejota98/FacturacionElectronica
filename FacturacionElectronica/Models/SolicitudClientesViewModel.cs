namespace FacturacionElectronica.Models
{
    public class SolicitudClientesViewModel
    {
        public int Documento { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public byte[] Rut { get; set; }
    }
}
