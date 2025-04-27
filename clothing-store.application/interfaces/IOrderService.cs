using clothing_store.application.Dtos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace clothing_store.application.interfaces
{
    public interface IOrderService
    {
        void CreatePendingOrder(int userId, decimal amount, string razorpayOrderId, string receipt, int addressId, List<OrderItemDto> cartItems);
        void ConfirmOrderPayment(string razorpayOrderId, string paymentId);
        void PlaceCodOrder(int userId, decimal amount, int addressId, List<OrderItemDto> cartItems);
        List<OrderDto> GetUserOrders(int userId);
        OrderDetailDto GetOrderDetails(int userId, int orderId);
        List<AdminOrderDto> GetAllOrders(string status = null);
        AdminOrderDetailDto GetOrderDetailsByAdmin(int orderId);


    }
}
