using AutoMapper;
using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Entities;
using ECommerce.DataAccess.Respository.Common;
using ECommerce.DataAccess.Respository.LanguageRepo;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.DataAccess.Respository.LanguageRepo
{
    public class LanguageRepository : Repository<Language>, ILanguageRepository
    {
        private readonly ECommerceDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IMapper _mapper;

        public LanguageRepository(ECommerceDbContext context, IWebHostEnvironment webHostEnvironment, IMapper mapper) : base(context)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _mapper = mapper;
        }
    }
}