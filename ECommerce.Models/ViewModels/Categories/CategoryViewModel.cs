using ECommerce.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Models.ViewModels.Categories
{
    public class CategoryViewModel : BaseCategoryViewModel
    {
        public bool IsShowOnHome { set; get; }

        public Status Status { set; get; }

        public string SeoTitle { set; get; }

        public string SeoAlias { set; get; }

        public string SeoDescription { set; get; }

        public string Language { set; get; }
    }
}