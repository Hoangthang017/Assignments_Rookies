﻿using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure;

namespace ECommerce.DataAccess.Repository.ProductRepo
{
    public class ProductRepository : Repository<Product>, IProductRepository
    {
        private readonly ECommerceDbContext _context;

        public ProductRepository(ECommerceDbContext context) : base(context)
        {
            _context = context;
        }
    }
}