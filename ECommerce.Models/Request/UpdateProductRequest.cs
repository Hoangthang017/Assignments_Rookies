using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Models.Request
{
    public class UpdateProductRequest
    {
        [Required]
        public int ProductId { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Detail { get; set; }

        [Required]
        public string LanguageId { get; set; }
    }
}