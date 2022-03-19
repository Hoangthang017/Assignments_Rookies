using ECommerce.DataAccess.Respository.Common;
using ECommerce.Models.Request.Common;
using ECommerce.Models.Request.Images;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.BackendApis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        [AllowAnonymous]
        public async Task<IActionResult> Create([FromForm] CreateUserImageRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var imageId = await _unitOfWork.Image.AddImage(request);
            if (imageId == 0)
                return BadRequest("Error to create the image");

            var image = _unitOfWork.Image.GetSingleById(imageId);
            if (image == null)
                return BadRequest("Cannot get the image view model");

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
                return BadRequest(ModelState);

            var imageId = await _unitOfWork.Image.AddImage(request);
            if (imageId == 0)
                return BadRequest("Error to create product image");

            var image = _unitOfWork.Image.GetSingleById(imageId);
            if (image == null)
                return BadRequest("Cannot get the image view model");

            return CreatedAtAction(
                nameof(GetById),
                new { id = imageId },
                image);
        }

        // GET
        // GET: api/images/user/5346D030-7824-4A41-9070-E7924B008D67
        [HttpGet("user/{userId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(string userId)
        {
            var imagePath = await _unitOfWork.Image.GetUserImagePathByUserId(userId);

            if (string.IsNullOrEmpty(imagePath))
                return BadRequest("Dont have any image path match in database");

            return Ok(imagePath);
        }

        // GET: api/images/paging/1?pageIndex=1&pagSize=1
        [HttpGet("paging/{productId}")]
        public async Task<IActionResult> GetAllPaging(int productId, [FromQuery] PagingRequestBase request)
        {
            var imageVMs = await _unitOfWork.Image.GetAllPaging(productId, request);
            if (imageVMs == null)
                return BadRequest("Dont have any image product in database");

            return Ok(imageVMs);
        }

        // GET: api/images/slide/1
        [HttpGet("slide/{take}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllSlide(int take)
        {
            var slideVMs = await _unitOfWork.Image.GetAllSlide(take);
            if (slideVMs == null)
                return BadRequest("Dont have any slide in database");

            return Ok(slideVMs);
        }

        // UPDATE
        // PUT: api/images/user
        [HttpPut("user")]
        [AllowAnonymous]
        public async Task<IActionResult> Update([FromForm] UpdateUserImageRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var isSucess = await _unitOfWork.Image.UpdateImage(request);
            if (!isSucess)
                return BadRequest("Error to update image");
            return Ok();
        }

        // PUT: api/images/product/1/1
        [HttpPut("product/{productId}/{imageId}")]
        public async Task<IActionResult> Update(int productId, int imageId, [FromForm] UpdateProductImageRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var isSucess = await _unitOfWork.Image.UpdateImage(imageId, productId, request);
            if (!isSucess)
                return BadRequest("Error to update product image");
            return Ok();
        }

        // DELETE
        // DELETE: api/images/1
        [HttpDelete("{imageId}")]
        public async Task<IActionResult> Delete(int imageId)
        {
            var isSuccess = await _unitOfWork.Image.DeleteImage(imageId);
            if (!isSuccess)
                return BadRequest("Error to delete image");
            return Ok();
        }
    }
}