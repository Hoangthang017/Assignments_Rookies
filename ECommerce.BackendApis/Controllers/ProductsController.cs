﻿using ECommerce.DataAccess.Respository.Common;
using ECommerce.Models.Request;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.BackendApis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductsController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // Create
        [HttpPost]
        public async Task<IActionResult> Create([FromForm] CreateProductRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            int productId = await _unitOfWork.Product.Create(request);
            if (productId == 0)
                return BadRequest();

            var product = _unitOfWork.Product.GetById(productId);

            return CreatedAtAction(nameof(_unitOfWork.Product.GetById),
                                   new { id = productId },
                                   product);
        }

        // Read
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _unitOfWork.Product.GetAll();
            if (products == null)
                return BadRequest();
            return Ok(products);
        }

        [HttpGet("{productId}")]
        public async Task<IActionResult> GetById(int productId)
        {
            var product = await _unitOfWork.Product.GetById(productId);

            if (product == null)
                return BadRequest($"Cannot find product with id: {productId}");
            return Ok(product);
        }

        // Update
        [HttpPatch]
        public async Task<IActionResult> Update([FromForm] UpdateProductRequest request)
        {
            var isSuccess = await _unitOfWork.Product.Update(request);
            if (!isSuccess)
                return BadRequest();
            return Ok();
        }

        // Delete
        [HttpDelete("{productId}")]
        public async Task<IActionResult> Delete(int productId)
        {
            var isSuccess = await _unitOfWork.Product.Remove(productId);
            if (!isSuccess)
                return BadRequest();
            return Ok();
        }
    }
}