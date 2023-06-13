using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Modelo
{
    public class Pagos
    {
        public int Id { get; set; }
        public int NumeroDocumento { get; set; }
        public int NumeroFactura { get; set; }
        public string Prefijo { get; set; }
        public int Total { get; set; }
        public int IdEstacionamiento { get; set; }
        public byte[] Imagen { get; set; }
        public bool Estado { get; set; }
        public string Fecha { get; set; }
    }
}
