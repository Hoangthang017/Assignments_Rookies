﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Models.Request.Images
{
    public class CreateUserImageRequest : CreateImageBaseRequest
    {
        public string UserId { get; set; }
    }
}