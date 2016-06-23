using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Data.Entities
{
    public class StockMovements
    {
        public int Id;
        public int ProductId;
        public int Quantity;
        public string TransactionType;
        public DateTime EntryDate;

    }
}
