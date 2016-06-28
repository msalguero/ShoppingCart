using System.Collections.Generic;
using ShoppingCart.Data.Entities;

namespace ShoppingCart.Data.Repositories
{
    public interface IShoppingCartRepository
    {
        List<ShopCart> GetPendingShopCarts(List<ShopCart> shopCartsList);
    }
}
