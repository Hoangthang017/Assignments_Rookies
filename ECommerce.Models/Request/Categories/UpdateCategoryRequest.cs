using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Models.Request.Categories
{
    public class UpdateCategoryRequest
    {
        public string Name { set; get; }

        public string SeoTitle { set; get; }

        public string SeoAlias { set; get; }

        public string SeoDescription { set; get; }
    }
}