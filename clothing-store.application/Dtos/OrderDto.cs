using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clothing_store.application.Dtos
{
    public class OrderDto
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string Address { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionId { get; set; }
    }
    public class OrderDetailDto
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string Address { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionId { get; set; }
        public List<OrderItemDto> Items { get; set; }
    }

    public class OrderItemDto
    {
        public string ProductName { get; set; }  // Added ProductName
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public int ProductId { get; set; }
        public int ProductVariantId { get; set; }
    }

    public class CreateOrderRequest
    {
        public decimal Amount { get; set; }
        public string Currency { get; set; }
        public string? Receipt { get; set; }
        public int UserId { get; set; }
        public int AddressId { get; set; }
        public List<OrderItemDto> CartItems { get; set; }
    }

    public class VerifyPaymentRequest
    {
        public string OrderId { get; set; }
        public string PaymentId { get; set; }
        public string Signature { get; set; }
    }

    public class AdminOrderDto
    {
        public int Id { get; set; }
        public string Status { get; set; }
        public decimal TotalAmount { get; set; }
        public string PaymentMethod { get; set; }
        public string TransactionId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerEmail { get; set; }
        public string Address { get; set; }
    }

    public class AdminOrderDetailDto : AdminOrderDto
    {
        public List<OrderItemDto> Items { get; set; }
    }

}
