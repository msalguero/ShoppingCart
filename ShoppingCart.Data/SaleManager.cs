using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Data
{
    public class SaleManager
    {
        private readonly IInventoryManager inventoryManager;
        private readonly IShoppingCartManager shoppingCartManager;

        public SaleManager(IInventoryManager _inventoryManager, IShoppingCartManager _shoppingCartManager)
        {
            inventoryManager = _inventoryManager;
            shoppingCartManager = _shoppingCartManager;
        }

        public float CheckOut(int cartId)
        {
            float CheckoutTotal = 0;

            var itemList = shoppingCartManager.getShoppingCartItems(cartId);
            if (inventoryManager.InStock(itemList))
            {
                foreach (var item in itemList)
                {
                    var itemPrice = inventoryManager.getProductPrice(item.ProductId);
                    var quantity = item.Quantity;
                    var itemTotalCost = quantity*itemPrice;

                    inventoryManager.decreaseStock(item.ProductId,quantity);
                    CheckoutTotal += itemTotalCost;
                }
            }
            else
            {
                throw new OutOfStockException();
            }

            shoppingCartManager.setCartAsPaid(cartId);
            return CheckoutTotal;
        }
    }
}
