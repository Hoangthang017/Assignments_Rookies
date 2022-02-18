﻿using ECommerce.DataAccess.Entities;
using ECommerce.DataAccess.Respository.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Respository.ProductRepo
{
    public interface IProductTranslationRepository : IRepository<ProductTranslation>
    {
        Task<ProductTranslation> GetByProductId(int id);
    }
}