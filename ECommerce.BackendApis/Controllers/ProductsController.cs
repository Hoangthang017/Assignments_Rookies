using AutoMapper;
using ECommerce.DataAccess.Respository.Common;
using ECommerce.Models.Request;
using ECommerce.Models.ViewModels;
using ECommerce.Utilities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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

        // Create
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            return Ok();
        }

        // Read
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            //var products = await _unitOfWork..GetAll();
            //if (products == null)
            //    return BadRequest();

            //// conver to product view models
            return Ok();
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetById(int productId)
        {
            //var product = await _unitOfWork.Product.GetById(productId);

            //if (product == null)
            //    return BadRequest($"Cannot find product with id: {productId}");
            return Ok();
        }

        // Update
        [HttpPatch]
        public async Task<IActionResult> Update([FromForm] UpdateProductRequest request)
        {
            //var isSuccess = await _unitOfWork.Product.Update(request);
            //if (!isSuccess)
            //    return BadRequest();
            return Ok();
        }

        // Delete
        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            //var isSuccess = await _unitOfWork.Product.Remove(productId);
            //if (!isSuccess)
            //    return BadRequest();
            return Ok();
        }
    }
}