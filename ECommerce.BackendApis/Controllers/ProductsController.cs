using AutoMapper;
using ECommerce.DataAccess.Respository.Common;
using ECommerce.Models.Request.Common;
using ECommerce.Models.Request.Products;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.BackendApis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public ProductsController(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        #region PRODUCT

        // CREATE
        // POST: api/products
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateProductRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var productId = await _unitOfWork.Product.Create(request);
            if (productId == 0)
                return BadRequest("Create product is unsuccess");

            var product = await _unitOfWork.Product.GetById(productId, request.LanguageId);
            if (product == null)
                return BadRequest("Cannot get product view model with id");

            return CreatedAtAction(nameof(GetById),
                                   new { id = productId },
                                   product);
        }

        // POST: api/products/1/review/asdasd-adasd
        [HttpPost("{productId}/review/{customerId}")]
        [AllowAnonymous]
        public async Task<IActionResult> CreateProductReview(int productId, string customerId, [FromBody] CreateProductReviewRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var reviewId = await _unitOfWork.Product.CreateProductReview(productId, customerId, request);
            if (reviewId == 0)
                return BadRequest("Create a review is unsuccess");

            var review = await _unitOfWork.Product.GetProductReviewById(reviewId);
            if (review == null)
                return BadRequest("Error to get review view model");

            return CreatedAtAction(nameof(GetProductReviewById),
                                   new { id = reviewId },
                                   review);
        }

        // READ
        // GET: api/products/paging/en-us?pageIndex=1&pagSize=1&categoryId=1
        [HttpGet("paging/{languageId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllPaging(string languageId, [FromQuery] GetProductPagingRequest request)
        {
            var productsVMs = await _unitOfWork.Product.GetAllPaging(languageId, request);

            if (productsVMs == null)
                return BadRequest("Dont have any product in database");

            return Ok(productsVMs);
        }

        // GET: api/products/featured/1/4/en-us
        [HttpGet("featured/{categoryId}/{take}/{languageId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFeaturedProduct(int categoryId, int take, string languageId)
        {
            var productsVMs = await _unitOfWork.Product.GetFeaturedProduct(languageId, take, categoryId);

            if (productsVMs == null)
                return BadRequest("Dont have any product feature in database");

            return Ok(productsVMs);
        }

        // GET: api/products/related/en-us/1/1/4
        [HttpGet("related/{languageId}/{productId}/{categoryId}/{take}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetRelatedProduct(string languageId, int productId, int categoryId, int take)
        {
            var productsVMs = await _unitOfWork.Product.GetRelatedProducts(languageId, productId, categoryId, take);

            if (productsVMs == null)
                return BadRequest("Dont have any related product in database");

            return Ok(productsVMs);
        }

        // GET: api/products/en-us/1
        [HttpGet("{languageId}/{productId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int productId, string languageId)
        {
            var productVM = await _unitOfWork.Product.GetById(productId, languageId);

            if (productVM == null)
                return BadRequest("Cannot get the product view model");

            return Ok(productVM);
        }

        // GET: api/products/1/review
        [HttpGet("{productId}/review")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllProductReview(int productId)
        {
            var reviewVMs = await _unitOfWork.Product.GetAllProductReview(productId);

            if (reviewVMs == null)
                return BadRequest("Dont have any product review for this product");

            return Ok(reviewVMs);
        }

        // GET: api/products/review/1
        [HttpGet("review/{reviewId}")]
        public async Task<IActionResult> GetProductReviewById(int reviewId)
        {
            var reviewVM = await _unitOfWork.Product.GetProductReviewById(reviewId);

            if (reviewVM == null)
                return BadRequest("Cannot get the review view model");

            return Ok(reviewVM);
        }

        // UPDATE
        // PUT: api/products/1/en-us
        [HttpPut("{productId}/{languageId}")]
        public async Task<IActionResult> Update(int productId, string languageId, [FromBody] UpdateProductRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var isSucess = await _unitOfWork.Product.Update(productId, languageId, request);
            if (isSucess == 0)
                return BadRequest("Error to update product");
            return Ok();
        }

        // PATCH: api/products/price/1?newPrice=321123
        [HttpPatch("{productId}/price/{newPrice}")]
        public async Task<IActionResult> UpdatePrice(int productId, decimal newPrice)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var isSucess = await _unitOfWork.Product.UpdatePrice(productId, newPrice);
            if (!isSucess)
                return BadRequest("Error to update price");
            return Ok();
        }

        // PATCH: api/products/1?quantity=32
        [HttpPatch("{productId}/quantity/{quantity}")]
        public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var isSucess = await _unitOfWork.Product.UpdateStock(productId, quantity);
            if (!isSucess)
                return BadRequest("Error to update quantity");
            return Ok();
        }

        // PATCH: api/viewcout/1
        [HttpPatch("viewcount/{productId}")]
        [AllowAnonymous]
        public async Task<IActionResult> Update(int productId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var isSucess = await _unitOfWork.Product.UpdateViewCount(productId);
            if (!isSucess)
                return BadRequest("Error to update view count");
            return Ok();
        }

        // DELETE
        // DELETE: api/products/1
        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            var isSuccess = await _unitOfWork.Product.Delete(productId);
            if (!isSuccess)
                return BadRequest("Error to delete product");
            return Ok();
        }

        // PATCH: api/products/deleteRange
        [HttpPatch("deleteRange")]
        public async Task<IActionResult> DeleteRange([FromBody] List<int> productIds)
        {
            var isSucess = await _unitOfWork.Product.DeleteRange(productIds);
            if (!isSucess)
                return BadRequest("Error to delete multiple products");
            return Ok();
        }

        #endregion PRODUCT
    }
}