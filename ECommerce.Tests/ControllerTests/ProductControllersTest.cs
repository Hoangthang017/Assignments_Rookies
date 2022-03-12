using AutoMapper;
using ECommerce.BackendApis.Controllers;
using ECommerce.DataAccess.Infrastructure;
using ECommerce.DataAccess.Repository.ProductRepo;
using ECommerce.DataAccess.Respository.Common;
using ECommerce.Models.Entities;
using ECommerce.Models.Request.Common;
using ECommerce.Models.Request.Products;
using ECommerce.Models.ViewModels.Common;
using ECommerce.Models.ViewModels.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace ECommerce.Tests.ControllerTests
{
    public class ProductControllersTest
    {
        private readonly Mock<IUnitOfWork> mockUnitOfWork = new Mock<IUnitOfWork>();
        private readonly IMapper _mapper;
        private readonly Random rand = new();

        public ProductControllersTest(IMapper mapper)
        {
            _mapper = mapper;
        }

        #region Product.Create

        [Fact]
        public async Task Create_WithModelStateError_ReturnBadRequest()
        {
            // Arrage
            var controller = new ProductsController(mockUnitOfWork.Object, _mapper);
            controller.ModelState.AddModelError("error", "some error");

            // Act
            var result = await controller.Create(It.IsAny<CreateProductRequest>());

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            mockUnitOfWork.Verify(x => x.Product.Create(It.IsAny<CreateProductRequest>()), Times.Never());
        }

        [Fact]
        public async Task Create_WithNoRequest_ReturnBadRequest()
        {
            // Arrage
            mockUnitOfWork.Setup(x => x.Product.Create(It.IsAny<CreateProductRequest>()))
                    .ReturnsAsync(0);
            var controller = new ProductsController(mockUnitOfWork.Object, _mapper);

            // Act
            var result = await controller.Create(It.IsAny<CreateProductRequest>());

            // Assert
            Assert.IsType<BadRequestResult>(result);
            mockUnitOfWork.Verify(x => x.Product.Create(It.IsAny<CreateProductRequest>()), Times.Once());
        }

        [Fact]
        public async Task Create_WithCreateProductRequest_ReturnProductViewModel()
        {
            //// Arrage
            //var sampleRequest = GetSampleCreateRequest();
            //var sampleResult = GetSampleResult(sampleRequest);

            //mockUnitOfWork.Setup(x => x.Product.Create(sampleRequest))
            //        .ReturnsAsync(sampleResult.Id);
            //mockUnitOfWork.Setup(x => x.Product.GetById(sampleResult.Id))
            //        .ReturnsAsync(sampleResult);

            //var controller = new ProductsController(mockUnitOfWork.Object, _mapper);

            //// Act
            //var createAtActionResult = await controller.Create(sampleRequest) as CreatedAtActionResult;

            //// Assert
            //var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(createAtActionResult);
            //var returnValue = Assert.IsType<ProductViewModel>(createdAtActionResult.Value);
            //mockUnitOfWork.Verify(x => x.Product.Create(sampleRequest), Times.Once);
            //mockUnitOfWork.Verify(x => x.Product.GetById(sampleResult.Id), Times.Once);
            //Assert.Equal(sampleResult.OriginalPrice, returnValue.OriginalPrice);
            //Assert.Equal(sampleResult.Name, returnValue.Name);
        }

        #endregion Product.Create

        //-------------------------------------------------------------------------------------------------

        #region Product.Get

        #region Product.GetProductById

        [Fact]
        public async Task GetProductById_WithUnExistValues_ReturnBadRequest()
        {
            //// Arrange
            //mockUnitOfWork.Setup(x => x.Product.GetById(It.IsAny<int>()))
            //              .ReturnsAsync(It.IsAny<ProductViewModel>);

            //var controller = new ProductsController(mockUnitOfWork.Object, _mapper);
            //// Act
            //var result = await controller.GetProductById(It.IsAny<int>());

            //// Assert
            //Assert.IsType<BadRequestResult>(result);
            //mockUnitOfWork.Verify(x => x.Product.GetById(It.IsAny<int>()), Times.Once());
        }

        [Fact]
        public async Task GetProductById_WithExistValues_ReturnProductViewModel()
        {
            //// Arrange
            //var sampleResult = GetSampleResult(It.IsAny<CreateProductRequest>());
            //mockUnitOfWork.Setup(x => x.Product.GetById(sampleResult.Id))
            //    .ReturnsAsync(sampleResult);
            //var controller = new ProductsController(mockUnitOfWork.Object, _mapper);

            //// Act
            //var actionResult = await controller.GetProductById(sampleResult.Id);

            //// Assert
            //var result = Assert.IsAssignableFrom<OkObjectResult>(actionResult);
            //var returnValue = Assert.IsType<ProductViewModel>(result.Value);
            //mockUnitOfWork.Verify(x => x.Product.GetById(sampleResult.Id), Times.Once());
            //Assert.Equal(sampleResult.OriginalPrice, returnValue.OriginalPrice);
            //Assert.Equal(sampleResult.Name, returnValue.Name);
        }

        #endregion Product.GetProductById

        //----------------------------------------------------

        #region Product.GetAll

        [Fact]
        public async Task GetAllPaging_WithUnExistValues_ReturnBadRequest()
        {
            //// Arrange
            //mockUnitOfWork.Setup(x => x.Product.GetAllPaging(It.IsAny<GetProductPagingRequest>()))
            //    .ReturnsAsync(It.IsAny<PageResult<ProductViewModel>>());

            //var controller = new ProductsController(mockUnitOfWork.Object, _mapper);
            //// Act
            //var result = await controller.GetAllPaging(It.IsAny<GetProductPagingRequest>());

            //// Assert
            //Assert.IsType<BadRequestResult>(result);
            //mockUnitOfWork.Verify(x => x.Product.GetAllPaging(It.IsAny<GetProductPagingRequest>()), Times.Once());
        }

        [Fact]
        public async Task GetAllPaging_WithPagingRequest_ReturnPageResult()
        {
            //// Arrange
            //// create sample request
            //var sampleRequest = new GetProductPagingRequest()
            //{
            //    CategoryId = rand.Next(10),
            //    //Keyword = Guid.NewGuid().ToString(),
            //    PageIndex = rand.Next(1, 10),
            //    PageSize = rand.Next(1, 5),
            //};

            //// get sample total records
            //var totalRecords = rand.Next(100);

            //// create list sample ProductViewModel with totalRecords rows
            //var items = new List<ProductViewModel>();
            //for (int i = 0; i < totalRecords; i++)
            //{
            //    items.Add(GetSampleResult(It.IsAny<CreateProductRequest>()));
            //}

            //// create sample result base on above values
            //var sampleResult = new PageResult<ProductViewModel>()
            //{
            //    Items = items,
            //    PageIndex = sampleRequest.PageIndex,
            //    PageSize = sampleRequest.PageSize,
            //    TotalRecords = totalRecords,
            //};

            //mockUnitOfWork.Setup(x => x.Product.GetAllPaging(sampleRequest))
            //    .ReturnsAsync(sampleResult)
            //    .Verifiable();

            //var controller = new ProductsController(mockUnitOfWork.Object, _mapper);

            //// Act
            //var actionResult = await controller.GetAllPaging(sampleRequest);

            //// Assert
            //var result = Assert.IsType<OkObjectResult>(actionResult);
            //var returnValue = Assert.IsType<PageResult<ProductViewModel>>(result.Value);
            //mockUnitOfWork.Verify(x => x.Product.GetAllPaging(sampleRequest), Times.Once());
            //Assert.Equal(sampleResult.Items, returnValue.Items);
            //Assert.Equal(sampleResult.PageIndex, returnValue.PageIndex);
            //Assert.Equal(sampleResult.PageSize, returnValue.PageSize);
            //Assert.Equal(sampleResult.TotalRecords, returnValue.TotalRecords);
        }

        #endregion Product.GetAll

        #endregion Product.Get

        //-------------------------------------------------------------------------------------------------

        #region Product.Update

        [Fact]
        public async Task Update_WithUnExistValues_ReturnBadRequest()
        {
            // Arrage
            mockUnitOfWork.Setup(x => x.Product.Update(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<UpdateProductRequest>()))
                .ReturnsAsync(0);

            var controller = new ProductsController(mockUnitOfWork.Object, _mapper);

            // Act
            var result = await controller.Update(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<UpdateProductRequest>());

            // Assert
            Assert.IsType<BadRequestResult>(result);
            mockUnitOfWork.Verify(x => x.Product.Update(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<UpdateProductRequest>()), Times.Once());
        }

        [Fact]
        public async Task Update_WithUpdateRequest_ReturnOkResult()
        {
            // Arrage
            var sampleProductId = rand.Next(10);
            var sampleLanguageId = "vi-VN";
            var sampleRequest = new UpdateProductRequest()
            {
                SeoAlias = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Details = Guid.NewGuid().ToString(),
                //Image = It.IsAny<IFormFile>(),
                Name = Guid.NewGuid().ToString(),
                SeoDescription = Guid.NewGuid().ToString(),
                SeoTitle = Guid.NewGuid().ToString(),
            };
            mockUnitOfWork.Setup(x => x.Product.Update(sampleProductId, sampleLanguageId, sampleRequest))
                .ReturnsAsync(1);

            var controller = new ProductsController(mockUnitOfWork.Object, _mapper);

            // Act
            var result = await controller.Update(sampleProductId, sampleLanguageId, sampleRequest);

            // Assert
            Assert.IsType<OkResult>(result);
            mockUnitOfWork.Verify(x => x.Product.Update(sampleProductId, sampleLanguageId, sampleRequest), Times.Once());
        }

        #endregion Product.Update

        //-------------------------------------------------------------------------------------------------

        #region Product.Update

        [Fact]
        public async Task Delete_WithUnExistValues_ReturnBadRequest()
        {
            // Arrage
            mockUnitOfWork.Setup(x => x.Product.Delete(It.IsAny<int>()))
                .ReturnsAsync(false);

            var controller = new ProductsController(mockUnitOfWork.Object, _mapper);

            // Act
            var result = await controller.Delete(It.IsAny<int>());

            // Assert
            Assert.IsType<BadRequestResult>(result);
            mockUnitOfWork.Verify(x => x.Product.Delete(It.IsAny<int>()), Times.Once());
        }

        [Fact]
        public async Task Delete_WithProductId_ReturnOkResult()
        {
            // Arrage
            var sampleProductId = rand.Next(10);
            mockUnitOfWork.Setup(x => x.Product.Delete(sampleProductId))
                .ReturnsAsync(true);

            var controller = new ProductsController(mockUnitOfWork.Object, _mapper);

            // Act
            var result = await controller.Delete(sampleProductId);

            // Assert
            Assert.IsType<OkResult>(result);
            mockUnitOfWork.Verify(x => x.Product.Delete(sampleProductId), Times.Once());
        }

        #endregion Product.Update

        //-------------------------------------------------------------------------------------------------

        #region Method Get sample values

        private CreateProductRequest GetSampleCreateRequest()
        {
            return new CreateProductRequest()
            {
                SeoAlias = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Details = Guid.NewGuid().ToString(),
                //Image = It.IsAny<IFormFile>(),
                LanguageId = "vi-VN",
                Name = Guid.NewGuid().ToString(),
                OriginalPrice = rand.Next(1000),
                Price = rand.Next(1000),
                SeoDescription = Guid.NewGuid().ToString(),
                SeoTitle = Guid.NewGuid().ToString(),
                Stock = rand.Next(100),
            };
        }

        private ProductViewModel GetSampleResult(CreateProductRequest request)
        {
            if (request == null)
            {
                return new ProductViewModel()
                {
                    Id = rand.Next(10),
                    Name = Guid.NewGuid().ToString(),
                    SeoAlias = Guid.NewGuid().ToString(),
                    //Categories = new List<string> { "test 1", "test 2" },
                    //CreatedDate = DateTime.Now,
                    Description = Guid.NewGuid().ToString(),
                    Details = Guid.NewGuid().ToString(),
                    LanguageId = "vi-VN",
                    OriginalPrice = rand.Next(1000),
                    Price = rand.Next(1000),
                    SeoDescription = Guid.NewGuid().ToString(),
                    SeoTitle = Guid.NewGuid().ToString(),
                    Stock = rand.Next(100),
                    //UpdatedDate = DateTime.Now,
                    ViewCount = rand.Next(10),
                };
            }

            return new ProductViewModel()
            {
                Id = rand.Next(),
                SeoAlias = Guid.NewGuid().ToString(),
                Description = Guid.NewGuid().ToString(),
                Details = Guid.NewGuid().ToString(),
                LanguageId = "vi-VN",
                Name = Guid.NewGuid().ToString(),
                OriginalPrice = rand.Next(1000),
                Price = rand.Next(1000),
                SeoDescription = Guid.NewGuid().ToString(),
                SeoTitle = Guid.NewGuid().ToString(),
                Stock = rand.Next(100),
                //Categories = new List<string> { "test 1", "test 2" },
                //CreatedDate = DateTime.Now,
                //UpdatedDate = DateTime.Now,
                ViewCount = rand.Next(10),
            };
        }

        #endregion Method Get sample values
    }
}