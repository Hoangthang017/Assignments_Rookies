using ECommerce.DataAccess.Respository.Common;
using ECommerce.Models.Enums;
using ECommerce.Models.Request.Categories;
using ECommerce.Models.Request.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.BackendApis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoriesController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // CREATE
        // POST: api/category
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CreateCategoryRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryId = await _unitOfWork.Category.Create(request);

            if (categoryId == 0)
                return BadRequest("create is unsuccess");

            var categoryVM = await _unitOfWork.Category.GetById(categoryId, request.languageId);

            return CreatedAtAction(
                        nameof(GetById),
                        new { id = categoryId },
                        categoryVM
                    );
        }

        // READ
        // GET: api/category/en-us
        [HttpGet("{languageId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllByName(string languageId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryAllName = await _unitOfWork.Category.GetAllName(languageId);
            if (categoryAllName == null)
                return BadRequest();

            return Ok(categoryAllName);
        }

        // GET: api/category/paging?PageIndex=1&PageSize=1&languageId=en-us
        [HttpGet("paging/{languageId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetAllPaging(string languageId, [FromQuery] GetCategoryPagingRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryVMs = await _unitOfWork.Category.GetAllPaging(languageId, request);
            if (categoryVMs == null)
                return BadRequest();

            return Ok(categoryVMs);
        }

        // GET: api/category/featured/4/en-us
        [HttpGet("featured/{take}/{languageId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetFeaturedCategory(int take, string languageId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryVMs = await _unitOfWork.Category.GetFeaturedCategory(languageId, take);
            if (categoryVMs == null)
                return BadRequest();

            return Ok(categoryVMs);
        }

        // GET: api/category?categoryId=1&languageId=en-us
        [HttpGet("{categoryId}/{languageId}")]
        [Authorize]
        public async Task<IActionResult> GetById(int categoryId, string languageId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var categoryVM = await _unitOfWork.Category.GetById(categoryId, languageId);
            if (categoryVM == null)
                return BadRequest();

            return Ok(categoryVM);
        }

        // UPDATE
        // PUT: api/category?categoryId=1&languageId=en-us
        [HttpPut("{categoryId}/{languageId}")]
        [Authorize]
        public async Task<IActionResult> Update(int categoryId, string languageId, [FromBody] UpdateCategoryRequest request)
        {
            var isSucess = await _unitOfWork.Category.Update(categoryId, languageId, request);
            if (!isSucess)
                return BadRequest();
            return Ok();
        }

        // PATCH: api/active/category?categoryId=1&status=1
        [HttpPatch("active/{categoryId}/{status}")]
        [Authorize]
        public async Task<IActionResult> UpdateActive(int categoryId, Status status)
        {
            var isSucess = await _unitOfWork.Category.UpdateActive(categoryId, status);
            if (!isSucess)
                return BadRequest();
            return Ok();
        }

        // PATCH: api/showOnHome/category?categoryId=1&showOnHome=true
        [HttpPatch("showOnHome/{categoryId}/{showOnHome}")]
        [Authorize]
        public async Task<IActionResult> UpdateShowHome(int categoryId, bool showOnHome)
        {
            var isSucess = await _unitOfWork.Category.UpdateShowOnHome(categoryId, showOnHome);
            if (!isSucess)
                return BadRequest();
            return Ok();
        }

        // DELETE
        // DELETE: api/category?categoryId=1
        [HttpDelete("{categoryId}")]
        [Authorize]
        public async Task<IActionResult> Delete(int categoryId)
        {
            var isSucess = await _unitOfWork.Category.Delete(categoryId);
            if (!isSucess)
                return BadRequest();
            return Ok();
        }

        // PATCH: api/category/deleteRange
        [HttpPatch("deleteRange")]
        [Authorize]
        public async Task<IActionResult> DeleteMulti([FromBody] List<int> categoryIds)
        {
            var isSucess = await _unitOfWork.Category.DeleteRange(categoryIds);
            if (!isSucess)
                return BadRequest();
            return Ok();
        }
    }
}