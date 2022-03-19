using ECommerce.DataAccess.Respository.Common;
using ECommerce.Models.Request.Common;
using ECommerce.Models.Request.Users;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.BackendApis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public UsersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

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

        //POST: api/users/register/admin
        [HttpPost("register/admin")]
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

        //POST: api/users/register
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
            if (userInfoVM == null)
                return BadRequest("Cannot get user information view modal");

            return CreatedAtAction(
                    nameof(GetById),
                    new { id = userId },
                    userInfoVM
                );
        }

        //GET: api/users/account
        [HttpGet("account")]
        public async Task<IActionResult> UserInfo()
        {
            var token = HttpContext.Request.Headers["Authorization"];

            var response = await _unitOfWork.User.GetUserInfo(token);
            if (response == null)
                return BadRequest("Cannot find the user");

            return Ok(response.Raw);
        }

        //GET: api/users/paging
        [HttpGet("paging")]
        public async Task<IActionResult> GetAllPaging([FromQuery] GetUserPagingRequest request)
        {
            var userInfoVMs = await _unitOfWork.User.GetAllPaging(request);
            if (userInfoVMs == null)
                return BadRequest("Error to get all user");

            return Ok(userInfoVMs);
        }

        //GET: api/users/adasd-asdasd
        [HttpGet("{userId}")]
        public async Task<IActionResult> GetById(string userId)
        {
            var response = await _unitOfWork.User.GetById(userId);
            if (response == null)
                return BadRequest("Cannot find the user");

            return Ok(response);
        }

        //PUT: api/users/213123-1231231asda
        [HttpPut("{userId}")]
        public async Task<IActionResult> Update(string userId, [FromBody] UpdateUserRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var isSucess = await _unitOfWork.User.UpdateUser(userId, request);
            if (!isSucess)
                return BadRequest("Update is unsuccess");
            return Ok();
        }

        //DELETE: api/users/213123-123123123as
        [HttpDelete("{userId}")]
        public async Task<IActionResult> Delete(string userId)
        {
            var isSucess = await _unitOfWork.User.RemoveUser(userId);
            if (!isSucess)
                return BadRequest("Remove user is unsuccess");
            return Ok();
        }

        //PATCH: api/users/deleteMulti
        [HttpPatch("deleteMulti")]
        public async Task<IActionResult> DeleteMulti([FromBody] List<string> userIds)
        {
            var isSucess = await _unitOfWork.User.RemoveRangeUser(userIds);
            if (!isSucess)
                return BadRequest("Remove all user with ids are unsuccess");
            return Ok();
        }
    }
}