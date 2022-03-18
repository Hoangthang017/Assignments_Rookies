using ECommerce.DataAccess.Respository.Common;
using ECommerce.Models.Request.Common;
using ECommerce.Models.Request.Users;
using ECommerce.Models.ViewModels.UserInfos;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.BackendApis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // login
        // POST: api/users/authenticate
        [HttpPost("authenticate")]
        [AllowAnonymous]
        public async Task<IActionResult> Authenticate([FromBody] LoginRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var resultToken = await _unitOfWork.User.Authencate(request);
            if (string.IsNullOrEmpty(resultToken))
                return BadRequest("User name or Password is incorrect");
            return Ok(new { token = resultToken });
        }

        // login
        [HttpPost("revoke")]
        [Authorize]
        public async Task<IActionResult> RevokeToken([FromBody] InforClientRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var token = HttpContext.Request.Headers["Authorization"];
            var resultToken = await _unitOfWork.User.RevokeToken(token, request);
            if (!resultToken)
                return BadRequest("Cannot revoke authorize token");
            return Ok();
        }

        // register
        [HttpPost("register/admin")]
        [Authorize]
        public async Task<IActionResult> RegisterAdmin([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = await _unitOfWork.User.CreateUser(request);

            if (string.IsNullOrEmpty(userId))
                return BadRequest("register is unsuccess");

            // create a admin account
            if (request.IsAdmin)
            {
                await _unitOfWork.User.UpdateRole(userId, "admin");
            }

            var userInfoVM = await _unitOfWork.User.GetById(userId);

            return CreatedAtAction(
                        nameof(GetById),
                        new { id = userId },
                        userInfoVM
                    );
        }

        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userId = await _unitOfWork.User.CreateUser(request);

            if (string.IsNullOrEmpty(userId))
                return BadRequest("register is unsuccess");

            var userInfoVM = await _unitOfWork.User.GetById(userId);

            return CreatedAtAction(
                    nameof(GetById),
                    new { id = userId },
                    userInfoVM
                );
        }

        // Get information
        [HttpGet("account")]
        [Authorize]
        public async Task<IActionResult> UserInfo()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var token = HttpContext.Request.Headers["Authorization"];

            var response = await _unitOfWork.User.GetUserInfo(token);
            if (response == null)
                return BadRequest();

            return Ok(response.Raw);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> GetAll()
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _unitOfWork.User.GetAll();
            if (response == null)
                return BadRequest();

            return Ok(response);
        }

        [HttpGet("paging")]
        [Authorize]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetUserPagingRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var userInfoVMs = await _unitOfWork.User.GetAllPaging(request);
            if (userInfoVMs == null)
                return BadRequest();

            return Ok(userInfoVMs);
        }

        [HttpGet("{userId}")]
        [Authorize]
        public async Task<IActionResult> GetById(string userId)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var response = await _unitOfWork.User.GetById(userId);
            if (response == null)
                return BadRequest();

            return Ok(response);
        }

        // update
        [HttpPut("{userId}")]
        [Authorize]
        public async Task<IActionResult> Update(string userId, [FromBody] UpdateUserRequest request)
        {
            var isSucess = await _unitOfWork.User.UpdateUser(userId, request);
            if (!isSucess)
                return BadRequest();
            return Ok();
        }

        // delete
        [HttpDelete("{userId}")]
        [Authorize]
        public async Task<IActionResult> Delete(string userId)
        {
            var isSucess = await _unitOfWork.User.RemoveUser(userId);
            if (!isSucess)
                return BadRequest();
            return Ok();
        }

        [HttpPatch("deleteMulti")]
        [Authorize]
        public async Task<IActionResult> DeleteMulti([FromBody] List<string> userIds)
        {
            var isSucess = await _unitOfWork.User.RemoveRangeUser(userIds);
            if (!isSucess)
                return BadRequest();
            return Ok();
        }
    }
}