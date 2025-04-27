using clothing_store.application.Dtos;
using clothing_store.application.interfaces;
using clothing_store.domain.models;
using clothing_store.infrastructure.Repositories;

namespace clothing_store.application.services
{
    public class OrderService : IOrderService
    {
        private readonly OrderRepository _orderRepository;

        public OrderService(OrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public void CreatePendingOrder(int userId, decimal amount, string razorpayOrderId, string receipt, int addressId, List<OrderItemDto> cartItems)
        {
            if (cartItems is null)
            {
                throw new ArgumentNullException(nameof(cartItems));
            }

            // Mapping OrderItemDto -> OrderItem
            var orderItems = cartItems.Select(item => new OrderItem
            {
                ProductId = item.ProductId,
                ProductVariantId = item.ProductVariantId,
                Quantity = item.Quantity,
                Price = item.Price
            }).ToList();

            _orderRepository.CreatePendingOrder(userId, amount, razorpayOrderId, receipt, addressId, orderItems);
        }

        public void ConfirmOrderPayment(string razorpayOrderId, string paymentId)
        {
            _orderRepository.ConfirmOrderPayment(razorpayOrderId, paymentId);
        }

        public void PlaceCodOrder(int userId, decimal amount, int addressId, List<OrderItemDto> cartItems)
        {
            if (cartItems is null)
            {
                throw new ArgumentNullException(nameof(cartItems));
            }

            var orderItems = cartItems.Select(item => new OrderItem
            {
                ProductId = item.ProductId,
                ProductVariantId = item.ProductVariantId,
                Quantity = item.Quantity,
                Price = item.Price
            }).ToList();

            _orderRepository.CreateCodOrder(userId, amount, addressId, orderItems);
        }

        public List<OrderDto> GetUserOrders(int userId)
        {
            // Fetch basic order details for the user
            var orders = _orderRepository.GetOrdersByUserId(userId);
            return orders.Select(order => new OrderDto
            {
                Id = order.Id,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                Address = $"{order.Address?.City}, {order.Address?.State},  {order.Address?.PostalCode}, {order.Address?.Country}",  // Concatenate address fields
                PaymentMethod = order.Payment?.Method,
                TransactionId = order.Payment?.TransactionId
            }).ToList();
        }

        public OrderDetailDto GetOrderDetails(int userId, int orderId)
        {
            var order = _orderRepository.GetOrderDetails(userId, orderId); // Fetch order by ID
            if (order == null) return null;

            return new OrderDetailDto
            {
                Id = order.Id,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                Address = $"{order.Address?.City}, {order.Address?.State},  {order.Address?.PostalCode}, {order.Address?.Country}",  // Concatenate address fields
                PaymentMethod = order.Payment?.Method,
                TransactionId = order.Payment?.TransactionId,
                Items = order.Items.Select(item => new OrderItemDto
                {   ProductId = item.ProductId,
                ProductVariantId = item.ProductVariantId,
                    ProductName = item.Product.Name,  // Map the product name
                    Price = item.Price,
                    Quantity = item.Quantity
                }).ToList()
            };
        }

        public List<AdminOrderDto> GetAllOrders(string status = null)
        {
            var orders = _orderRepository.GetAllOrders();

            if (!string.IsNullOrEmpty(status))
            {
                orders = orders.Where(o => o.Status.Equals(status, StringComparison.OrdinalIgnoreCase)).ToList();
            }

            return orders.Select(order => new AdminOrderDto
            {
                Id = order.Id,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                PaymentMethod = order.Payment?.Method,
                TransactionId = order.RazorpayOrderId,
                CustomerName = order.User.FullName,
                CustomerEmail = order.User.Email,
                Address = $"{order.Address?.City}, {order.Address?.State},  {order.Address?.PostalCode}, {order.Address?.Country}",
            }).ToList();
        }

        public AdminOrderDetailDto GetOrderDetailsByAdmin(int orderId)
        {
            var order = _orderRepository.GetOrderWithDetails(orderId);
            if (order == null) return null;

            return new AdminOrderDetailDto
            {
                Id = order.Id,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                PaymentMethod = order.Payment?.Method,
                TransactionId = order.Payment?.TransactionId,
                CustomerName = order.User.FullName,
                CustomerEmail = order.User.Email,
                Address = $"{order.Address?.City}, {order.Address?.State},  {order.Address?.PostalCode}, {order.Address?.Country}",
                Items = order.Items.Select(item => new OrderItemDto
                {
                    ProductName = item.Product.Name,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    ProductId = item.Product.Id,
                    ProductVariantId = item.ProductVariantId,
                }).ToList()
            };
        }
    }

}
