using ECommerce.DataAccess.Repository.ImageRepo;
using ECommerce.DataAccess.Repository.ProductRepo;
using ECommerce.DataAccess.Repository.UserRepo;

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

        #region User

        IUserRepository User { get; }

        #endregion User

        IImageRepository Image { get; }

        // save method
        Task<bool> Save();
    }
}