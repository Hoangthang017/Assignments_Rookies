using ECommerce.DataAccess.Respository.Common;
using ECommerce.Models.Request.Orders;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace ECommerce.BackendApis.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class OrdersController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public OrdersController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        // CREATE
        // POST: api/order/1
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Create(CreateOrderRequest request)
        {
            var customerId = new Guid();

            // check has loged account => handle to get customer current Id
            var token = HttpContext.Request.Headers["Authorization"];
            if (!string.IsNullOrEmpty(token))
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(token.FirstOrDefault("").Split(" ")[1]);
                if (jwtSecurityToken == null)
                    return BadRequest("Fail to decode jwt token");
                customerId = new Guid(jwtSecurityToken.Claims.First(claim => claim.Type == "sub").Value);
            }

            var orderId = await _unitOfWork.Order.Create(customerId, request);
            if (orderId == 0)
                return BadRequest("Fail to add order");

            var orderVM = await GetById(orderId, customerId.ToString());
            if (orderVM == null) return BadRequest("Cannot load the order view model");

            return CreatedAtAction(nameof(GetById), new { id = orderId }, orderVM);
        }

        // READ
        // GET: api/order/asdasd-asasdasd/1
        [HttpGet("{customerId}/{orderId}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(int orderId, string customerId)
        {
            var orderVM = await _unitOfWork.Order.GetById(new Guid(customerId), orderId);
            if (orderVM == null) return BadRequest("Cannot find the order");
            return Ok(orderVM);
        }
    }
}