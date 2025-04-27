using clothing_store.application.Dtos;
using clothing_store.application.interfaces;
using clothing_store.application.services;
using Microsoft.AspNetCore.Mvc;

namespace clothing_store.api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentController : ControllerBase
    {
        private readonly IRazorpayService _razorpayService;
        private readonly IOrderService _orderService;
        private readonly IEmailService _emailService;

        public PaymentController(IRazorpayService razorpayService,IOrderService orderService, IEmailService emailService)
        {
            _razorpayService = razorpayService;
            _orderService = orderService;
            _emailService = emailService;
        }

        [HttpPost("create-order")]
        public IActionResult CreateOrder([FromBody] CreateOrderRequest request)
        {
            var order = _razorpayService.CreateOrder(request.Amount, request.Currency, request.Receipt);
            var orderId = order["id"].ToString();
            _orderService.CreatePendingOrder(request.UserId, request.Amount, orderId, request.Receipt, request.AddressId, request.CartItems);
            return Ok(new { orderId });
        }

        [HttpPost("verify")]
        public IActionResult VerifyPayment([FromBody] VerifyPaymentRequest request)
        {
            bool isValid = _razorpayService.VerifySignature(request.OrderId, request.PaymentId, request.Signature);
            if (isValid)
            {
                // Update order status in database
                _orderService.ConfirmOrderPayment(request.OrderId, request.PaymentId);
                return Ok(new { status = "Payment verified successfully." });
            }
            return BadRequest(new { status = "Payment verification failed." });
        }

        [HttpPost("send-order-confirmation")]
        public async Task<IActionResult> SendOrderConfirmation([FromBody] OrderEmailRequest request)
        {
            var result = await _emailService.SendOrderConfirmationEmailAsync(request);
            if (result)
                return Ok();
            else
                return BadRequest("Failed to send email");
        }

        [HttpPost("cod-order")]
        public IActionResult PlaceCodOrder([FromBody] CreateOrderRequest request)
        {
            _orderService.PlaceCodOrder(request.UserId, request.Amount, request.AddressId, request.CartItems);
            return Ok(new { status = "COD order placed successfully." });
        }

    }
}
