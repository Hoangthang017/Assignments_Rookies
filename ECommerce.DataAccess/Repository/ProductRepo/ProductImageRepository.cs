using AutoMapper;
using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.DataAccess.Infrastructure.Common;
using ECommerce.Models.Entities;
using ECommerce.Models.Request.Images;
using ECommerce.Models.ViewModels.ProductImages;
using ECommerce.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Headers;

namespace ECommerce.DataAccess.Repository.ProductRepo
{
    public class ProductImageRepository : Repository<Image>, IProductImageRepository
    {
        private readonly ECommerceDbContext _context;
        private readonly IMapper _mapper;
        private readonly IStorageService _storageService;

        public ProductImageRepository(ECommerceDbContext context, IStorageService storageService, IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
            _storageService = storageService;
        }
    }
}