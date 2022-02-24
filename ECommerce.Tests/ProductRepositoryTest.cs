using AutoMapper;
using ECommerce.BackendApis.Controllers;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.DataAccess.Repository.ProductRepo;
using ECommerce.DataAccess.Respository.Common;
using ECommerce.Models.Entities;
using ECommerce.Models.Request.Products;
using ECommerce.Models.ViewModels.Products;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Testing;
using Moq;
using System;
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

        [Fact]
        public async Task Create_WithNoRequest_ReturnBadRequest()
        {
            // Arrage
            var controller = new ProductsController(unitOfWorkMock.Object, _mapper);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = controller.Create(request: null);

            // Assert
            var actionResult = Assert.IsType<ActionResult<ProductViewModel>>(result);
            Assert.IsType<BadRequestObjectResult>(actionResult.Result);
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

            var sampleResult = new ProductViewModel();
            var product = new Mock<IRepository<Product>>();
            unitOfWorkMock.Setup(x => x.Product.Create(sampleRequest))
                    .ReturnsAsync(1);

            var controller = new ProductsController(unitOfWorkMock.Object, _mapper);
            // Act

            var result = await controller.Create(sampleRequest);

            // Assert
            Assert.IsType<BadRequestResult>(result);
        }
    }
}