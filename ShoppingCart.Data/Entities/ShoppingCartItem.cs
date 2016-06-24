using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Data.Entities
{
    public class ShoppingCartItem
    {
        public int Id;
        public int ShoppingCartId ;
        public int ProductId;
        public int Quantity;
    }
}
