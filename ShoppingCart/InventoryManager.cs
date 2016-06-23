using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.Data.Repositories;

namespace ShoppingCart
{
    public class InventoryManager
    {
        private readonly IStockMovementsRepository _stockRepository;

        public InventoryManager(IStockMovementsRepository stockRepository)
        {
            _stockRepository = stockRepository;
        }
    }
}
