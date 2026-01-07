using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CapaEntidad
{
    public class Venta
    {
        public int IdVenta { get; set; }
        public int IdUsuario { get; set; }
        public string IdTransaccion { get; set; }
        public string FechaTexto { get; set; }
        public decimal MontoTotal { get; set; }
        public string Direccion { get; set; }
        public List<DetalleVenta> oDetalleVenta { get; set; }
    }
}
