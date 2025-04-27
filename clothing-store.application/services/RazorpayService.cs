using clothing_store.application.interfaces;
using Razorpay.Api;
using System.Text;

namespace clothing_store.application.services
{
    public class RazorpayService : IRazorpayService
    {
        private readonly string _key;
        private readonly string _secret;

        public RazorpayService(string key, string secret)
        {
            _key = key;
            _secret = secret;
        }

        public Order CreateOrder(decimal amount, string currency, string receipt)
        {
            RazorpayClient client = new RazorpayClient(_key, _secret);
            var options = new Dictionary<string, object>
        {
            { "amount", amount * 100 }, // amount in paise
            { "currency", currency },
            { "receipt", receipt },
            { "payment_capture", 1 }
        };
            return client.Order.Create(options);
        }

        public bool VerifySignature(string orderId, string paymentId, string signature)
        {
            string payload = orderId + "|" + paymentId;
            string expectedSignature;
            using (var hmac = new System.Security.Cryptography.HMACSHA256(Encoding.UTF8.GetBytes(_secret)))
            {
                byte[] hash = hmac.ComputeHash(Encoding.UTF8.GetBytes(payload));
                expectedSignature = BitConverter.ToString(hash).Replace("-", "").ToLower();
            }

            return expectedSignature == signature;
        }
    }
}
