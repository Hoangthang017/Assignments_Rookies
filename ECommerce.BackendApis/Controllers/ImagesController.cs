using ECommerce.DataAccess.Respository.Common;
using ECommerce.Models.Request.Common;
using ECommerce.Models.Request.Images;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.BackendApis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ImagesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // CREATE
        // POST: api/images/user
        [HttpPost("user")]
        public async Task<IActionResult> Create([FromForm] CreateUserImageRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var imageId = await _unitOfWork.Image.AddImage(request);

            if (imageId == 0)
                return BadRequest();

            var image = _unitOfWork.Image.GetSingleById(imageId);

            return CreatedAtAction(
                nameof(GetById),
                new { id = imageId },
                image);
        }

        // POST: api/images/product
        [HttpPost("product")]
        public async Task<IActionResult> Create([FromForm] CreateProductImageRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var imageId = await _unitOfWork.Image.AddImage(request);

            if (imageId == 0)
                return BadRequest();

            var image = _unitOfWork.Image.GetSingleById(imageId);

            return CreatedAtAction(
                nameof(GetById),
                new { id = imageId },
                image);
        }

        // GET
        // GET: api/images/user/5346D030-7824-4A41-9070-E7924B008D67
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetById(string userId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var imagePath = await _unitOfWork.Image.GetUserImagePathByUserId(userId);

            if (string.IsNullOrEmpty(imagePath))
                return BadRequest();

            return Ok(imagePath);
        }

        // GET: api/images/paging?pageIndex=1&pagSize=1
        [HttpGet("paging/{productId}")]
        public async Task<IActionResult> GetAllPaging(int productId, [FromQuery] PagingRequestBase request)
        {
            var productsVMs = await _unitOfWork.Image.GetAllPaging(productId, request);

            if (productsVMs == null)
                return BadRequest();

            return Ok(productsVMs);
        }

        // UPDATE
        // PUT: api/images/user
        [HttpPut("user")]
        public async Task<IActionResult> Update([FromForm] UpdateUserImageRequest request)
        {
            var isSucess = await _unitOfWork.Image.UpdateImage(request);
            if (!isSucess)
                return BadRequest();
            return Ok();
        }

        // PUT: api/images/product/1/1
        [HttpPut("product/{productId}/{imageId}")]
        public async Task<IActionResult> Update(int productId, int imageId, [FromForm] UpdateProductImageRequest request)
        {
            var isSucess = await _unitOfWork.Image.UpdateImage(imageId, productId, request);
            if (!isSucess)
                return BadRequest();
            return Ok();
        }

        // DELETE
        // DELETE: api/images/1
        [HttpDelete("{imageId}")]
        public async Task<IActionResult> Delete(int imageId)
        {
            var isSuccess = await _unitOfWork.Image.DeleteImage(imageId);
            if (!isSuccess)
                return BadRequest();
            return Ok();
        }
    }
}