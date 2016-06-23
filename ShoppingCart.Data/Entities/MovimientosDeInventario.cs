using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Data.Entities
{
    public class MovimientosDeInventario
    {
        public int Id;
        public int ProductoId;
        public int Cantidad;
        public string TipoTransaccion;
        public DateTime FechaDeIngreso;

    }
}
