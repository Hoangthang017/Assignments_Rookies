using AutoMapper;
using ECommerce.DataAccess.Respository.Common;
using ECommerce.Models.Request.Common;
using ECommerce.Models.Request.ProductImages;
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

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var productId = await _unitOfWork.Product.Create(request);

            if (productId == 0)
                return BadRequest();

            var product = _unitOfWork.Product.GetById(productId);

            return CreatedAtAction(nameof(GetProductById),
                                   new { id = productId },
                                   product);
        }

        // GET

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

        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetProductPagingRequest request)
        {
            var productsVMs = await _unitOfWork.Product.GetAllPaging(request);

            if (productsVMs == null)
                return BadRequest();

            return Ok(productsVMs);
        }

        [HttpGet("{productId}/{languageId}")]
        [Authorize]
        public async Task<IActionResult> Get(int productId, string languageId)
        {
            // get data
            var productTranslation = await _unitOfWork.ProductTranslation
                .GetSingleByCondition(x => x.ProductId == productId && x.LanguageId == languageId, new string[] { "Product" });

            if (productTranslation == null)
                return BadRequest();
            // mapper

            var productVM = ECommerceMapper.Map<ProductViewModel>(_mapper, productTranslation, productTranslation.Product);

            return Ok(productVM);
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetProductById(int productId)
        {
            var image = await _unitOfWork.Product.GetById(productId);

            if (image == null)
                return BadRequest();

            return Ok(image);
        }

        // UPDATE

        [HttpPut("{productId}/{languageId}")]
        public async Task<IActionResult> Update(int productId, string languageId, [FromForm] UpdateProductRequest request)
        {
            var isSucess = await _unitOfWork.Product.Update(productId, languageId, request);
            if (isSucess == 0)
                return BadRequest();
            return Ok();
        }

        [HttpPatch("price/{productId}")]
        public async Task<IActionResult> Update(int productId, decimal newPrice)
        {
            var isSucess = await _unitOfWork.Product.UpdatePrice(productId, newPrice);
            if (!isSucess)
                return BadRequest();
            return Ok();
        }

        [HttpPatch("stock/{productId}")]
        public async Task<IActionResult> Update(int productId, int Quantity)
        {
            var isSucess = await _unitOfWork.Product.UpdateStock(productId, Quantity);
            if (!isSucess)
                return BadRequest();
            return Ok();
        }

        [HttpPatch("viewcount/{productId}")]
        public async Task<IActionResult> Update(int productId)
        {
            var isSucess = await _unitOfWork.Product.UpdateViewCount(productId);
            if (!isSucess)
                return BadRequest();
            return Ok();
        }

        // DELETE

        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            var isSuccess = await _unitOfWork.Product.Delete(productId);
            if (!isSuccess)
                return BadRequest();
            return Ok();
        }

        #endregion PRODUCT

        #region IMAGE

        [HttpGet("{productId}/images/{imageId}")]
        public async Task<IActionResult> GetImageById(int productId, int imageId)
        {
            var image = await _unitOfWork.ProductImage.GetImageById(imageId);

            if (image == null)
                return BadRequest();

            return Ok(image);
        }

        [HttpPost("{productId}/images")]
        public async Task<IActionResult> Create(int productId, [FromForm] CreateProductImageRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var imageId = await _unitOfWork.ProductImage.AddImage(productId, request);
            if (imageId == 0)
                return BadRequest();

            var image = await _unitOfWork.ProductImage.GetImageById(imageId);

            return CreatedAtAction(nameof(GetImageById),
                                   new { id = imageId },
                                   image);
        }

        [HttpPut("{productId}/images/{imageId}")]
        public async Task<IActionResult> UpdateImage(int imageId, [FromForm] UpdateProductImageRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _unitOfWork.ProductImage.UpdateImage(imageId, request);

            if (result == 0)
                return BadRequest();

            return Ok();
        }

        [HttpDelete("{productId}/images/{imageId}")]
        public async Task<IActionResult> DeleteImage(int imageId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _unitOfWork.ProductImage.RemoveImage(imageId);

            if (result == 0)
                return BadRequest();

            return Ok();
        }

        #endregion IMAGE
    }
}