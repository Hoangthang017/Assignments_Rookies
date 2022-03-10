using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Models.Request.Images
{
    public class CreateImageBaseRequest
    {
        public string Caption { get; set; }
        public int SortOrder { get; set; }

        public IFormFile ImageFile { get; set; }
    }
}