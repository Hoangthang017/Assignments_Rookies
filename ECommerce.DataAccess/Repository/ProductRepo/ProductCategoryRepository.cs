﻿using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Entities;

namespace ECommerce.DataAccess.Repository.ProductRepo
{
    public class ProductCategoryRepository : Repository<ProductCategory>, IProductCategoryRepository
    {
        private readonly ECommerceDbContext _context;

        public ProductCategoryRepository(ECommerceDbContext context) : base(context)
        {
            _context = context;
        }

        public IEnumerable<ProductCategory> GetByAlias(string alias)
        {
            return _context.ProductCategories.Where(pt => pt.Alias == alias);
        }
    }
}