using Razorpay.Api;

namespace clothing_store.application.interfaces
{
    public interface IRazorpayService
    {
        Order CreateOrder(decimal amount, string currency, string receipt);
        bool VerifySignature(string orderId, string paymentId, string signature);
    }
}
