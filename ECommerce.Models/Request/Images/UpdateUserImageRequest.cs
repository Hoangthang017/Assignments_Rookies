using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Models.Request.Images
{
    public class UpdateUserImageRequest : UpdateImageBaseRequest
    {
        public string UserId { get; set; }
    }
}