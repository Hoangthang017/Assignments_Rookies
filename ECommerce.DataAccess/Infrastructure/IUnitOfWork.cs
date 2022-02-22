using ECommerce.DataAccess.Repository.ProductRepo;

namespace ECommerce.DataAccess.Respository.Common
{
    public interface IUnitOfWork
    {
        #region Product

        IProductRepository Product { get; }

        IProductTranslationRepository ProductTranslation { get; }

        IProductImageRepository ProductImage { get; }

        IProductInCategoryRepository ProductInCategory { get; }

        IProductReviewRepository ProductReview { get; }

        #endregion Product

        ICategoryRepository Category { get; }
        ICategoryTranslationRepository CategoryTranslation { get; }

        // save method
        Task<bool> Save();
    }
}