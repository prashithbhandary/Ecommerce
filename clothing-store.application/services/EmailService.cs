using clothing_store.application.Dtos;
using clothing_store.application.interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace clothing_store.application.services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<bool> SendOrderConfirmationEmailAsync(OrderEmailRequest request)
        {
            var email = new MimeMessage();
            email.From.Add(MailboxAddress.Parse(_configuration["EmailSettings:FromEmail"]));
            email.To.Add(MailboxAddress.Parse(request.ToEmail));
            email.Subject = "Order Confirmation - Clothing Store";

            var builder = new BodyBuilder();
            builder.HtmlBody = GenerateEmailHtml(request);
            email.Body = builder.ToMessageBody();

            using var smtp = new SmtpClient();
            await smtp.ConnectAsync(_configuration["EmailSettings:SmtpServer"], 587, false);
            await smtp.AuthenticateAsync(_configuration["EmailSettings:Username"], _configuration["EmailSettings:Password"]);
            await smtp.SendAsync(email);
            await smtp.DisconnectAsync(true);

            return true;
        }

        private string GenerateEmailHtml(OrderEmailRequest request)
        {
            var itemsHtml = string.Join("", request.CartItems.Select(item => $@"
            <tr>
                <td style='padding:8px;border-bottom:1px solid #ddd;'>{item.ProductName}</td>
                <td style='text-align:center;padding:8px;border-bottom:1px solid #ddd;'>{item.Quantity}</td>
                <td style='text-align:right;padding:8px;border-bottom:1px solid #ddd;'>₹{item.Price}</td>
            </tr>
        "));

            return $@"
        <html>
        <body style='font-family:Arial,sans-serif;background-color:#f5f5f5;padding:20px;'>
            <div style='max-width:600px;margin:auto;background:#fff;padding:20px;border-radius:8px;'>
                <h2 style='text-align:center;color:#333;'>Thank you for your order!</h2>

                <h3>Shipping Address</h3>
                <p>
                    {request.FullName}<br>
                    {request.ShippingAddress.AddressLine1}<br>
                    {request.ShippingAddress.AddressLine2}<br>
                    {request.ShippingAddress.City}, {request.ShippingAddress.State} - {request.ShippingAddress.ZipCode}<br>
                    {request.ShippingAddress.Country}<br>
                    Phone: {request.ShippingAddress.PhoneNumber}
                </p>

                <h3>Order Summary</h3>
                <table style='width:100%;border-collapse:collapse;'>
                    <thead>
                        <tr style='background-color:#f0f0f0;'>
                            <th style='text-align:left;padding:8px;'>Product</th>
                            <th style='text-align:center;padding:8px;'>Quantity</th>
                            <th style='text-align:right;padding:8px;'>Price</th>
                        </tr>
                    </thead>
                    <tbody>
                        {itemsHtml}
                    </tbody>
                </table>

                <h3 style='text-align:right;'>Total Amount: ₹{request.TotalAmount}</h3>

                <div style='text-align:center;margin-top:30px;'>
                    <p style='font-size:14px;color:#555;'>You will receive another email once your items are shipped.</p>
                    <p style='font-size:14px;color:#555;'>Thank you for shopping with us!</p>
                </div>
            </div>
        </body>
        </html>";
        }
    }
}
