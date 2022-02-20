using ECommerce.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Models.ViewModels
{
    public class ProductCategoryViewModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Alias { get; set; }

        public string Description { get; set; }

        public int? DisplayOrder { get; set; }

        public int? ParentId { get; set; }

        public string Image { get; set; }

        public bool? HomeFlag { get; set; }

        public IEnumerable<ProductViewModel> ProductViewModels { get; set; }
    }
}