using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clothing_store.application.Dtos
{
    public class OrderEmailRequest
    {
        public string ToEmail { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public AddressDto ShippingAddress { get; set; }
        public List<CartItemDto> CartItems { get; set; }
        public decimal TotalAmount { get; set; }
    }

    public class AddressDto
    {
        public string AddressLine1 { get; set; } = string.Empty;
        public string? AddressLine2 { get; set; }
        public string City { get; set; } = string.Empty;
        public string State { get; set; } = string.Empty;
        public string ZipCode { get; set; } = string.Empty;
        public string Country { get; set; } = string.Empty;
        public string PhoneNumber { get; set; } = string.Empty;
    }

    public class CartItemDto
    {
        public string ProductName { get; set; } = string.Empty;
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }

}
