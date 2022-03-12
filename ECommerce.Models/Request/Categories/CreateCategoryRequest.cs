using ECommerce.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Models.Request.Categories
{
    public class CreateCategoryRequest
    {
        public int SortOrder { set; get; }

        public bool IsShowOnHome { set; get; }

        public int? ParentId { set; get; }

        public Status Status { set; get; }

        public string Name { set; get; }

        public string SeoTitle { set; get; }

        public string SeoAlias { set; get; }

        public string SeoDescription { set; get; }

        public string languageId { get; set; }
    }
}