using ECommerce.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Models.Request.Products
{
    public class CreateProductReviewRequest
    {
        public Rating Rating { get; set; }

        public string Comment { get; set; }
    }
}