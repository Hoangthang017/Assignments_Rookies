using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Models.Request.Users
{
    public class InforClientRequest
    {
        public string ClientId { get; set; }
        public string ClientSecret { get; set; }

        public string Scope { get; set; }
    }
}