using AutoMapper;
using ECommerce.DataAccess.Respository.Common;
using ECommerce.Models.Entities;
using ECommerce.Models.Request;
using ECommerce.Models.ViewModels;
using ECommerce.Utilities;
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

        // GET: api/<ProductsController>
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _unitOfWork.Products.GetAll(new string[] { "ProductCategory" }).ToListAsync();
            var productVMs = ECommerceMapper.Map<List<ProductViewModel>>(_mapper, products);
            if (productVMs == null)
                return BadRequest();
            return Ok(productVMs);
        }

        // GET api/<ProductsController>/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var product = await _unitOfWork.Products.GetSingleById(id);
            if (product == null)
                return BadRequest();
            return Ok(product);
        }

        // POST api/<ProductsController>
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductRequest request)
        {
            var newProduct = ECommerceMapper.Map<Product>(_mapper, request);
            _unitOfWork.Products.Update(newProduct);
            var isSucess = await _unitOfWork.Save();
            if (!isSucess)
                return BadRequest();
            return Ok();
        }

        // PUT api/<ProductsController>/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromForm] UpdateProductRequest request)
        {
            var newProduct = ECommerceMapper.Map<Product>(_mapper, request);
            var product = await _unitOfWork.Products.GetSingleById(id);
            newProduct.Id = id;
            product = newProduct;
            var isSucess = await _unitOfWork.Save();
            if (!isSucess)
                return BadRequest();
            return Ok();
        }

        // DELETE api/<ProductsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            Product product = await _unitOfWork.Products.GetSingleById(id);
            if (product == null)
                return BadRequest();
            _unitOfWork.Products.Delete(product);
            var isSuccess = await _unitOfWork.Save();
            if (!isSuccess)
                return BadRequest();
            return Ok();
        }
    }
}