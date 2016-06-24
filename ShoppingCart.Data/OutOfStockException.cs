using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Data
{
    public class OutOfStockException : Exception
    {
        public OutOfStockException() { }

        public OutOfStockException(string message) : base(message){}
    }
}
