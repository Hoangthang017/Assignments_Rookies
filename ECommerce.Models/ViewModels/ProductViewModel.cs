using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Models.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        public decimal Price { get; set; }

        public decimal OriginalPrice { get; set; }

        public int Quantity { get; set; }

        public int ViewCount { get; set; }

        public DateTime DateCreated { get; set; }

        public string Name { set; get; }

        public string Description { set; get; }

        public string Detail { set; get; }

        public string LanguageId { set; get; }
    }
}