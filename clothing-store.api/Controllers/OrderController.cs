using clothing_store.application.interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;

namespace clothing_store.api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        // This will automatically extract the user ID from the JWT token
        private int GetUserIdFromToken()
        {
            var token = Request.Headers["Authorization"].ToString().Replace("Bearer ", "");
            if (string.IsNullOrEmpty(token))
                return 0;  // or handle error if token is absent

            var handler = new JwtSecurityTokenHandler();
            var jwtToken = handler.ReadJwtToken(token);
            var userIdClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "sub");

            return userIdClaim != null ? int.Parse(userIdClaim.Value) : 0;
        }

        // Get User Orders
        [HttpGet("user-orders")]
        [Authorize]
        public IActionResult GetUserOrders()
        {
            int userId = GetUserIdFromToken();
            var orders = _orderService.GetUserOrders(userId);
            return Ok(orders);
        }

        // Get Order Details
        [HttpGet("order-details/{orderId}")]
        [Authorize]
        public IActionResult GetOrderDetails(int orderId)
        {
            int userId = GetUserIdFromToken();
            var orderDetails = _orderService.GetOrderDetails(userId, orderId);
            if (orderDetails == null)
                return NotFound();

            return Ok(orderDetails);
        }

        [HttpGet("admin-orders")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetAllOrders([FromQuery] string? status)
        {
            var orders = _orderService.GetAllOrders(status);
            return Ok(orders);
        }

        [HttpGet("admin-order-details/{orderId}")]
        [Authorize(Roles = "Admin")]
        public IActionResult GetOrderDetailsByAdmin(int orderId)
        {
            var orderDetails = _orderService.GetOrderDetailsByAdmin(orderId);
            if (orderDetails == null)
                return NotFound();

            return Ok(orderDetails);
        }

    }
}
