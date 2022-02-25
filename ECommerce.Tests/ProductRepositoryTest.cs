using AutoMapper;
using ECommerce.BackendApis.Controllers;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.DataAccess.Repository.ProductRepo;
using ECommerce.DataAccess.Respository.Common;
using ECommerce.Models.Entities;
using ECommerce.Models.Request.Products;
using ECommerce.Models.ViewModels.Products;
using Microsoft.AspNetCore.Mvc;
using Moq;
using Neleus.LambdaCompare;
using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Tests
{
    public class ProductRepositoryTest
    {
        private readonly Mock<IUnitOfWork> unitOfWorkMock = new Mock<IUnitOfWork>();
        private readonly Mock<IProductRepository> _productRepository;

        private readonly IMapper _mapper;
        private readonly Random rand = new();

        public ProductRepositoryTest(IMapper mapper)
        {
            _mapper = mapper;
        }

        #region Product.Create

        [Fact]
        public async Task Create_WithModelStateError_ReturnBadRequest()
        {
            // Arrage
            var controller = new ProductsController(unitOfWorkMock.Object, _mapper);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await controller.Create(request: null);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Create_WithNoRequest_ReturnBadRequest()
        {
            // Arrage

            var sampleRequest = new CreateProductRequest()
            {
                SeoAlias = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Details = Guid.NewGuid().ToString(),
                Image = null,
                LanguageId = "vi-VN",
                Name = Guid.NewGuid().ToString(),
                OriginalPrice = rand.Next(1000),
                Price = rand.Next(1000),
                SeoDescription = Guid.NewGuid().ToString(),
                SeoTitle = Guid.NewGuid().ToString(),
                Stock = rand.Next(100),
            };
            var sampleResult = new ProductViewModel();
            var product = new Mock<IRepository<Product>>();
            unitOfWorkMock.Setup(x => x.Product.Create(sampleRequest))
                    .ReturnsAsync(0);
            var controller = new ProductsController(unitOfWorkMock.Object, _mapper);

            // Act
            var result = await controller.Create(sampleRequest);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Create_WithCreateProductRequest_ReturnProductViewModel()
        {
            // Arrage
            var sampleRequest = new CreateProductRequest()
            {
                SeoAlias = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Details = Guid.NewGuid().ToString(),
                Image = null,
                LanguageId = "vi-VN",
                Name = Guid.NewGuid().ToString(),
                OriginalPrice = rand.Next(1000),
                Price = rand.Next(1000),
                SeoDescription = Guid.NewGuid().ToString(),
                SeoTitle = Guid.NewGuid().ToString(),
                Stock = rand.Next(100),
            };

            unitOfWorkMock.Setup(x => x.Product.Create(sampleRequest))
                    .ReturnsAsync(1);

            var controller = new ProductsController(unitOfWorkMock.Object, _mapper);
            // Act

            var createdAtActionResult = await controller.Create(sampleRequest) as CreatedAtActionResult;
            var result = createdAtActionResult.Value as ProductViewModel;

            // Assert
            Assert.IsType<ProductViewModel>(result);
            Assert.Equal(sampleRequest.Name, result.Name);
        }

        #endregion Product.Create

        #region Product.Get

        [Fact]
        public async Task Get_WithUnExistValues_ReturnBadRequest()
        {
            // Arrange
            var sampleProdcutId = 10;
            var sampleLanguageId = "vi-vn";
            unitOfWorkMock.Setup(x => x.ProductTranslation
                          .GetSingleByCondition(x => x.ProductId == sampleProdcutId &&
                                                    x.LanguageId == sampleLanguageId,
                                                    new string[] { "Product" }))
                          .ReturnsAsync(It.IsAny<ProductTranslation>);

            var controller = new ProductsController(unitOfWorkMock.Object, _mapper);
            // Act
            var result = await controller.Get(sampleProdcutId, sampleLanguageId);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }

        [Fact]
        public async Task Get_WithExistValues_ReturnProductViewModel()
        {
            // Arrange
            var productId = 1;
            var languageId = "vi-VN";
            var sampleResult = new ProductTranslation()
            {
                Id = rand.Next(0, 10),
                LanguageId = languageId,
                SeoAlias = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Details = Guid.NewGuid().ToString(),
                Name = Guid.NewGuid().ToString(),
                SeoDescription = Guid.NewGuid().ToString(),
                SeoTitle = Guid.NewGuid().ToString(),
                ProductId = productId,
                Product = new Product()
                {
                    Id = productId,
                    OriginalPrice = rand.Next(10000),
                    Price = rand.Next(10000),
                    Stock = rand.Next(100),
                    UpdatedDate = DateTime.Now,
                    ViewCount = rand.Next(10)
                }
            };

            //unitOfWorkMock.Setup(x => x.ProductTranslation
            //              .GetSingleByCondition(It.Is<Expression<Func<ProductTranslation, bool>>>(expression =>
            //Lambda.Eq(expression, tc => tc.Id == sampleResult.ProductId))))
            //              .ReturnsAsync(sampleResult);

            var controller = new ProductsController(unitOfWorkMock.Object, _mapper);
            // Act
            var result = await controller.Get(sampleResult.ProductId, sampleResult.LanguageId);

            // Assert
            Assert.IsAssignableFrom<ViewResult>(result);
        }

        #endregion Product.Get
    }
}