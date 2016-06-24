using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.Data.Entities;

namespace ShoppingCart.Data
{
    public interface IShoppingCartManager
    {
        List<ShoppingCartItem> getShoppingCartItems(int cartId);
        void setCartAsPaid(int cartId);
    }
}
