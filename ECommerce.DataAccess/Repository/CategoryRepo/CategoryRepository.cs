using AutoMapper;
using ECommerce.DataAccess.EF;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.Models.Entities;
using ECommerce.Models.Enums;
using ECommerce.Models.Request.Categories;
using ECommerce.Models.Request.Common;
using ECommerce.Models.ViewModels.Categories;
using ECommerce.Models.ViewModels.Common;
using ECommerce.Utilities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.DataAccess.Repository.ProductRepo
{
    public class CategoryRepository : Repository<Category>, ICategoryRepository
    {
        private readonly ECommerceDbContext _context;
        private readonly IMapper _mapper;

        public CategoryRepository(
            ECommerceDbContext context,
            IMapper mapper) : base(context)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<int> Create(CreateCategoryRequest request)
        {
            var category = ECommerceMapper.Map<Category>(_mapper, request);

            var categoryTranslation = ECommerceMapper.Map<CategoryTranslation>(_mapper, request);

            category.CatogeryTranslations = new List<CategoryTranslation>() { categoryTranslation };

            await _context.Categories.AddAsync(category);

            await _context.SaveChangesAsync();

            return category.Id;
        }

        public async Task<List<BaseCategoryViewModel>> GetActiveCategory(string languageId)
        {
            var categories = await (from c in _context.Categories
                                    join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                                    where c.Status == Status.Active && ct.LanguageId == languageId
                                    select (new BaseCategoryViewModel()
                                    {
                                        Id = c.Id,
                                        Name = ct.Name
                                    })).ToListAsync();
            if (categories == null)
                throw new ECommerceException("Faild to get all name categories");

            return categories;
        }

        public async Task<IEnumerable<BaseCategoryViewModel>> GetAllName(string languageId)
        {
            var categories = await _context.CategoryTranslations
                .Where(x => x.LanguageId == languageId)
                .Select(x => new BaseCategoryViewModel()
                {
                    Name = x.Name,
                    Id = x.CategoryId
                }).ToListAsync();
            if (categories == null)
                throw new ECommerceException("Faild to get all name categories");

            return categories;
        }

        public async Task<PageResult<CategoryViewModel>> GetAllPaging(string languageId, GetCategoryPagingRequest request)
        {
            // get list user
            var query = from c in _context.Categories
                        join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                        join l in _context.Languages on ct.LanguageId equals l.Id
                        where l.Id == languageId
                        select new { c, ct, languageName = l.Name };

            // paging
            int totalRow = await query.CountAsync();
            var data = query.Skip((request.PageIndex - 1) * request.PageSize)
                            .Take(request.PageSize);

            // conver to viewmodel
            var categoryVMs = data.Select(x => new CategoryViewModel()
            {
                Id = x.c.Id,
                SeoAlias = x.ct.SeoAlias,
                IsShowOnHome = x.c.IsShowOnHome,
                Language = x.languageName,
                Name = x.ct.Name,
                SeoDescription = x.ct.SeoDescription,
                SeoTitle = x.ct.SeoTitle,
                Status = x.c.Status
            });

            // select and projection
            var pageResult = new PageResult<CategoryViewModel>()
            {
                PageIndex = request.PageIndex,
                PageSize = request.PageSize,
                TotalRecords = totalRow,
                Items = categoryVMs
            };

            return pageResult;
        }

        public async Task<CategoryViewModel> GetById(int categoryId, string languageId)
        {
            var categoryVM = await (from c in _context.Categories
                                    join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                                    join l in _context.Languages on ct.LanguageId equals l.Id
                                    where c.Id == categoryId && ct.LanguageId == languageId
                                    select new CategoryViewModel()
                                    {
                                        Id = c.Id,
                                        IsShowOnHome = c.IsShowOnHome,
                                        Status = c.Status,
                                        SeoAlias = ct.SeoAlias,
                                        Name = ct.Name,
                                        SeoDescription = ct.SeoDescription,
                                        SeoTitle = ct.SeoTitle,
                                        Language = l.Name
                                    }).FirstOrDefaultAsync();
            if (categoryVM == null)
                throw new ECommerceException("Cannot find the category");

            return categoryVM;
        }

        public async Task<List<BaseCategoryViewModel>> GetFeaturedCategory(string languageId, int take)
        {
            var categories = await (from c in _context.Categories
                                    join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                                    where c.IsShowOnHome == true && ct.LanguageId == languageId
                                    select (new BaseCategoryViewModel()
                                    {
                                        Id = c.Id,
                                        Name = ct.Name
                                    })).Take(take).ToListAsync();
            if (categories == null)
                throw new ECommerceException("Faild to get all name categories");

            return categories;
        }

        public async Task<bool> Update(int categoryId, string languageId, UpdateCategoryRequest request)
        {
            var category = await (from c in _context.Categories
                                  join ct in _context.CategoryTranslations on c.Id equals ct.CategoryId
                                  where c.Id == categoryId && ct.LanguageId == languageId
                                  select ct).FirstOrDefaultAsync();

            if (category == null)
                throw new ECommerceException("Cannot find the category");

            category.SeoAlias = request.SeoAlias;
            category.Name = request.Name;
            category.SeoDescription = request.SeoDescription;
            category.SeoTitle = request.SeoTitle;

            _context.CategoryTranslations.Update(category);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateActive(int categoryId, Status status)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null)
                throw new ECommerceException("Cannot find the category");
            category.Status = status;
            _context.Categories.Update(category);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> UpdateShowOnHome(int categoryId, bool showOnHome)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null)
                throw new ECommerceException("Cannot find the category");
            category.IsShowOnHome = showOnHome;
            _context.Categories.Update(category);
            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> Delete(int categoryId)
        {
            var category = await _context.Categories.FindAsync(categoryId);
            if (category == null)
                throw new ECommerceException("Cannot find the category");
            _context.Categories.Remove(category);

            return await _context.SaveChangesAsync() > 0;
        }

        public async Task<bool> DeleteRange(List<int> categoryIds)
        {
            var categories = _context.Categories.Where(x => categoryIds.Contains(x.Id)).ToList();
            _context.Categories.RemoveRange(categories);
            return await _context.SaveChangesAsync() > 0;
        }
    }
}