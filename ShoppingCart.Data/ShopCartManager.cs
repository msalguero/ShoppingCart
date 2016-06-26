﻿using System.Collections.Generic;
using ShoppingCart.Data.Entities;
using ShoppingCart.Data.Repositories;

namespace ShoppingCart.Data
{
    public class ShopCartManager
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        public ShopCartManager(IShoppingCartRepository shoppingCartRepository)
        {
            _shoppingCartRepository = shoppingCartRepository;
        }

        public List<ShopCart> GetCarritosPendientes(List<ShopCart> listaCarritos)
        {
            return _shoppingCartRepository.GetCarritosPendientes(listaCarritos);
        }
    }
}
