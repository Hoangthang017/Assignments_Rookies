using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Models.Request.Orders
{
    public class BaseProductOrderRequest
    {
        public decimal Price { get; set; }

        public int Quantity { get; set; }
    }
}