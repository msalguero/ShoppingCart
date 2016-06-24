using System.Collections.Generic;
using ShoppingCart.Data.Entities;

namespace ShoppingCart.Data
{
    public interface IInventoryManager
    {
        float getProductPrice(int productId);
        void decreaseStock(int productId, int quantity);
        void sendEmail(List<Product> itemList);
        List<Product> getOutOfStockItems(List<ShoppingCartItem> itemList);
    }
}