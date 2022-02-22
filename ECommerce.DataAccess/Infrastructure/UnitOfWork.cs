using AutoMapper;
using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Repository.ProductRepo;
using ECommerce.DataAccess.Respository.Common;

namespace ECommerce.DataAccess.Infrastructure
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ECommerceDbContext _context;
        private readonly IMapper _mapper;

        public UnitOfWork(ECommerceDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;

            Product = new ProductRepository(context, _mapper);

            ProductImage = new ProductImageRepository(context);

            ProductInCategory = new ProductInCategoryRepository(context);

            ProductReview = new ProductReviewRepository(context);

            ProductTranslation = new ProductTranslationRepository(context);

            Category = new CategoryRepository(context);

            CategoryTranslation = new CategoryTranslationRepository(context);
        }

        #region Product

        public IProductRepository Product { get; private set; }

        public IProductImageRepository ProductImage { get; private set; }

        public IProductInCategoryRepository ProductInCategory { get; private set; }

        public IProductReviewRepository ProductReview { get; private set; }

        public IProductTranslationRepository ProductTranslation { get; private set; }

        #endregion Product

        public ICategoryRepository Category { get; private set; }

        public ICategoryTranslationRepository CategoryTranslation { get; private set; }

        // save method
        async Task<bool> IUnitOfWork.Save()
        {
            return await _context.SaveChangesAsync() > 0;
        }
    }
}