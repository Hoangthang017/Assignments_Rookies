using ECommerce.Models.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Models.Request.Common
{
    public class GetPagingRequest : PagingRequestBase
    {
        public string Keyword { get; set; }
    }
}