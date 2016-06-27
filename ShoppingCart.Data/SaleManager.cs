using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.Data.Entities;

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

            if (shoppingCartManager.isCartPaid(cartId))
            {
                throw new CartAlreadyPaidException();
            }

            var itemList = shoppingCartManager.getShoppingCartItems(cartId);
            var itemsOutOfStock = inventoryManager.getOutOfStockItems(itemList);
            if (itemsOutOfStock.Count <= 0)
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
                inventoryManager.sendEmail(itemsOutOfStock);
                throw new OutOfStockException();
            }

            shoppingCartManager.setCartAsPaid(cartId);
            return CheckoutTotal;
        }
    }
}
