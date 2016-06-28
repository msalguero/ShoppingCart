using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShoppingCart.Data
{
    public class CartAlreadyPaidException : Exception
    {
        public CartAlreadyPaidException() { }

        public CartAlreadyPaidException(string message) : base(message){}
    }
}
