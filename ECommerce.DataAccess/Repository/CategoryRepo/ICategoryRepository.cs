using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Entities;
using ECommerce.Models.Enums;
using ECommerce.Models.Request.Categories;
using ECommerce.Models.Request.Common;
using ECommerce.Models.ViewModels.Categories;
using ECommerce.Models.ViewModels.Common;

namespace ECommerce.DataAccess.Repository.ProductRepo
{
    public interface ICategoryRepository : IRepository<Category>
    {
        Task<int> Create(CreateCategoryRequest request);

        Task<IEnumerable<BaseCategoryViewModel>> GetAllName(string languageId);

        Task<PageResult<CategoryViewModel>> GetAllPaging(string languageId, GetCategoryPagingRequest request);

        Task<List<BaseCategoryViewModel>> GetActiveCategory(string languageId);

        Task<List<BaseCategoryViewModel>> GetFeaturedCategory(string languageId, int take);

        Task<CategoryViewModel> GetById(int categoryId, string languageId);

        Task<bool> Update(int categoryId, string languageId, UpdateCategoryRequest request);

        Task<bool> UpdateShowOnHome(int categoryId, bool showOnHome);

        Task<bool> UpdateActive(int categoryId, Status status);

        Task<bool> Delete(int categoryId);

        Task<bool> DeleteRange(List<int> categoryIds);
    }
}