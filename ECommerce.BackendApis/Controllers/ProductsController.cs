using AutoMapper;
using ECommerce.DataAccess.Respository.Common;
using ECommerce.Models.Request.Common;
using ECommerce.Models.Request.Images;
using ECommerce.Models.Request.Products;
using ECommerce.Models.ViewModels.Products;
using ECommerce.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ECommerce.BackendApis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
            {
                return BadRequest(ModelState);
            }

            var productId = await _unitOfWork.Product.Create(request);

            if (productId == 0)
                return BadRequest();

            var product = await _unitOfWork.Product.GetById(productId, request.LanguageId);

            return CreatedAtAction(nameof(GetById),
                                   new { id = productId },
                                   product);
        }

        // READ
        // GET: api/products
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            // get all product
            var productTranslations = _unitOfWork.ProductTranslation.GetAll(new string[] { "Product" });

            // mapper product translation and product to ProductViewModel
            var productsVMs = new List<ProductViewModel>();
            await productTranslations.ForEachAsync(x =>
                productsVMs.Add(ECommerceMapper.Map<ProductViewModel>(_mapper, x.Product, x))
            );

            // check
            if (productsVMs == null)
                return BadRequest();

            return Ok(productsVMs);
        }

        // GET: api/products/paging?pageIndex=1&pagSize=1&categoryId=1
        [HttpGet("paging/{languageId}")]
        public async Task<IActionResult> GetAllPaging(string languageId, [FromQuery] GetProductPagingRequest request)
        {
            var productsVMs = await _unitOfWork.Product.GetAllPaging(languageId, request);

            if (productsVMs == null)
                return BadRequest();

            return Ok(productsVMs);
        }

        // GET: api/products/featured/1/4/en-us
        [HttpGet("featured/{categoryId}/{take}/{languageId}")]
        public async Task<IActionResult> GetFeaturedProduct(int categoryId, int take, string languageId)
        {
            var productsVMs = await _unitOfWork.Product.GetFeaturedProduct(languageId, take, categoryId);

            if (productsVMs == null)
                return BadRequest();

            return Ok(productsVMs);
        }

        [HttpGet("{productId}/{languageId}")]
        public async Task<IActionResult> GetById(int productId, string languageId)
        {
            var image = await _unitOfWork.Product.GetById(productId, languageId);

            if (image == null)
                return BadRequest();

            return Ok(image);
        }

        // UPDATE
        // PUT: api/products/1/en-us
        [HttpPut("{productId}/{languageId}")]
        public async Task<IActionResult> Update(int productId, string languageId, [FromBody] UpdateProductRequest request)
        {
            var isSucess = await _unitOfWork.Product.Update(productId, languageId, request);
            if (isSucess == 0)
                return BadRequest();
            return Ok();
        }

        // PATCH: api/products/price/1?newPrice=321123
        [HttpPatch("{productId}/price/{newPrice}")]
        public async Task<IActionResult> UpdatePrice(int productId, decimal newPrice)
        {
            var isSucess = await _unitOfWork.Product.UpdatePrice(productId, newPrice);
            if (!isSucess)
                return BadRequest();
            return Ok();
        }

        // PATCH: api/products/1?quantity=32
        [HttpPatch("{productId}/quantity/{quantity}")]
        public async Task<IActionResult> UpdateQuantity(int productId, int quantity)
        {
            var isSucess = await _unitOfWork.Product.UpdateStock(productId, quantity);
            if (!isSucess)
                return BadRequest();
            return Ok();
        }

        // PATCH: api/viewcout/1
        [HttpPatch("viewcount/{productId}")]
        public async Task<IActionResult> Update(int productId)
        {
            var isSucess = await _unitOfWork.Product.UpdateViewCount(productId);
            if (!isSucess)
                return BadRequest();
            return Ok();
        }

        // DELETE
        // DELETE: api/products/1
        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            var isSuccess = await _unitOfWork.Product.Delete(productId);
            if (!isSuccess)
                return BadRequest();
            return Ok();
        }

        // PATCH: api/products/deleteRange
        [HttpPatch("deleteRange")]
        [Authorize]
        public async Task<IActionResult> DeleteRange([FromBody] List<int> productIds)
        {
            var isSucess = await _unitOfWork.Product.DeleteRange(productIds);
            if (!isSucess)
                return BadRequest();
            return Ok();
        }

        #endregion PRODUCT
    }
}