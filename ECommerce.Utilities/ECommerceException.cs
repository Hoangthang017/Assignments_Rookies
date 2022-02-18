using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Utilities
{
    public class ECommerceException : Exception
    {
        public ECommerceException()
        {
        }

        public ECommerceException(string message)
            : base(message)
        {
        }

        public ECommerceException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}