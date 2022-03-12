using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Models.Request.Common
{
    public class GetProductPagingRequest : PagingRequestBase
    {
        //public string Keyword { get; set; }

        public int CategoryId { get; set; }
    }
}