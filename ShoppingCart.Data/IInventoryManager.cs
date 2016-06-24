using System.Collections.Generic;
using ShoppingCart.Data.Entities;

namespace ShoppingCart.Data
{
    public interface IInventoryManager
    {
        bool InStock(List<ShoppingCartItem> itemlist);
        float getProductPrice(int productId);
        void decreaseStock(int productId, int quantity);
    }
}