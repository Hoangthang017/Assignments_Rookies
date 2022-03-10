using ECommerce.DataAccess.Respository.Common;
using ECommerce.Models.Request.Images;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.BackendApis.Controllers
{
    [Route("api")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public ImageController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // CREATE
        // api/user/avatar
        [HttpPost("user/avatar")]
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

        // GET

        [HttpGet("user/{userId}/avatar")]
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

        // UPDATE
        [HttpPut("user/avatar")]
        public async Task<IActionResult> Update([FromForm] UpdateUserImageRequest request)
        {
            var isSucess = await _unitOfWork.Image.UpdateImage(request);
            if (!isSucess)
                return BadRequest();
            return Ok();
        }

        // DELETE
        [HttpDelete("user/avatar/{imageId}")]
        public async Task<IActionResult> Delete(int imageId)
        {
            var isSuccess = await _unitOfWork.Image.DeleteImage(imageId);
            if (!isSuccess)
                return BadRequest();
            return Ok();
        }
    }
}