using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Models.ViewModels.Common
{
    public class PageResult<T> : PageResultBase
    {
        public IEnumerable<T> Items { get; set; }
    }
}