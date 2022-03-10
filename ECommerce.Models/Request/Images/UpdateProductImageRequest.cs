using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Models.Request.Images
{
    public class UpdateProductImageRequest : UpdateImageBaseRequest
    {
        public int ProductId { get; set; }
        public bool IsDefault { get; set; }
    }
}