using Braintree;

namespace TorKartingowyCoreMVC.Services
{
    public class BraintreeService : IBraintreeService
    {
        private IBraintreeGateway BraintreeGateway { get; set; }

        //private readonly IConfiguration _config;

        //public BraintreeService(IConfiguration config)
        //{
        //    _config = config;
        //}

        public IBraintreeGateway CreateGateway()
        {
            return new BraintreeGateway()
            {
                Environment = Braintree.Environment.SANDBOX,
                MerchantId = "n3zd856n7vrbjwtn",
                PublicKey = "k7b8f2k6cxsmrtd4",
                PrivateKey = "8a3f79a7e3ee5e4b2062ac65b8802eca"
            };
        }

        public IBraintreeGateway GetGateway()
        {
            if (BraintreeGateway == null)
            {
                BraintreeGateway = CreateGateway();
            }
            return BraintreeGateway;
        }
    }
}
