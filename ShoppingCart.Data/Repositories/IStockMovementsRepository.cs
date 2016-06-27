using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ShoppingCart.Data.Entities;

namespace ShoppingCart.Data.Repositories
{
    public interface IStockMovementsRepository
    {
        List<StockMovements> GetMovements();
    }
}
