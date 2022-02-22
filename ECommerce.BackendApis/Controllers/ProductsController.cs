using AutoMapper;
using ECommerce.DataAccess.Respository.Common;
using ECommerce.Models.Request;
using ECommerce.Models.ViewModels;
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

        // CREATE

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var isSucess = await _unitOfWork.Product.Create(request);
            if (isSucess == 0)
                return BadRequest();
            return Ok();
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
            Product product = await _unitOfWork.Product.GetSingleById(productId);
            if (product == null)
                return BadRequest();
            _unitOfWork.Product.Delete(product);
            var isSuccess = await _unitOfWork.Save();
            if (!isSuccess)
                return BadRequest();
            return Ok();
        }
    }
}