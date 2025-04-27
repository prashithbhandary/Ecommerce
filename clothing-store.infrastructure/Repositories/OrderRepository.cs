
using clothing_store.domain.models;
using clothing_store.infrastructure.context;
using Microsoft.EntityFrameworkCore;

namespace clothing_store.infrastructure.Repositories
{
    public class OrderRepository
    {
        private readonly dbContext _context;

        public OrderRepository(dbContext context)
        {
            _context = context;
        }

        // Create pending order and payment
        public Order CreatePendingOrder(int userId, decimal amount, string razorpayOrderId, string receipt, int addressId, List<OrderItem> orderItems)
        {
            var order = new Order
            {
                UserId = userId,
                TotalAmount = amount,
                Status = "Pending",
                OrderDate = DateTime.UtcNow,
                RazorpayOrderId = razorpayOrderId,
                ReceiptId = receipt,
                AddressId = addressId,
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            var payment = new Payment
            {
                OrderId = order.Id,
                Method = "Razorpay",
                IsPaid = false,
                TransactionId = razorpayOrderId,
                AmountPaid = 0
            };

            _context.Payments.Add(payment);
            _context.SaveChanges();

            // Now Add OrderItems
            foreach (var orderItem in orderItems)
            {
                orderItem.OrderId = order.Id; // important to assign OrderId here
                _context.OrderItems.Add(orderItem);
            }
            _context.SaveChanges();

            return order;
        }


        // Confirm payment
        public void ConfirmOrderPayment(string razorpayOrderId, string paymentId)
        {
            var payment = _context.Payments.FirstOrDefault(p => p.TransactionId == razorpayOrderId);
            if (payment != null)
            {
                payment.IsPaid = true;
                payment.PaidAt = DateTime.UtcNow;

                var order = _context.Orders.First(o => o.Id == payment.OrderId);
                payment.AmountPaid = order.TotalAmount;
                payment.TransactionId = paymentId; // now update it with payment_id

                order.Status = "Paid";

                _context.SaveChanges();
            }
        }

        public Order CreateCodOrder(int userId, decimal amount, int addressId, List<OrderItem> orderItems)
        {
            var order = new Order
            {
                UserId = userId,
                TotalAmount = amount,
                Status = "Confirmed", // COD orders are already confirmed
                OrderDate = DateTime.UtcNow,
                AddressId = addressId,
            };

            _context.Orders.Add(order);
            _context.SaveChanges();

            foreach (var orderItem in orderItems)
            {
                orderItem.OrderId = order.Id;
                _context.OrderItems.Add(orderItem);
            }
            _context.SaveChanges();

            return order;
        }

        // Method to get orders by user ID
        public List<Order> GetOrdersByUserId(int userId)
        {
            return _context.Orders
                .Where(order => order.UserId == userId)
                .Include(order => order.Address) // Include address if needed
                .Include(order => order.Payment) // Include payment details if needed
                .ToList();
        }

        // Method to get order details by user ID and order ID
        public Order GetOrderDetails(int userId, int orderId)
        {
            return _context.Orders
                .Where(order => order.UserId == userId && order.Id == orderId)
                .Include(order => order.Address) // Include address
                .Include(order => order.Payment) // Include payment details
                .Include(order => order.Items) // Include order items (use 'Items' instead of 'OrderItems')
                .ThenInclude(orderItem => orderItem.Product) // Include product details for each order item
                .FirstOrDefault();
        }

        // In clothing_store.infrastructure.repositories

        public List<Order> GetAllOrders()
        {
            return _context.Orders.Include(o => o.User).ToList();
        }

        public Order GetOrderWithDetails(int orderId)
        {
            return _context.Orders
    .Include(o => o.User)
    .Include(o => o.Items)
        .ThenInclude(oi => oi.Product)
            .ThenInclude(p => p.Variants)
    .FirstOrDefault(o => o.Id == orderId);

        }

    }
}
