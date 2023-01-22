using Braintree;

namespace TorKartingowyCoreMVC.Services
{
    public class BraintreeService : IBraintreeService
    {
        private readonly IConfiguration _config;

        public BraintreeService(IConfiguration config)
        {
            _config = config;
        }

        public IBraintreeGateway CreateGateway()
        {
            var newGateway = new BraintreeGateway()
            {
                Environment = Braintree.Environment.SANDBOX,
                MerchantId = "mwvfb4wggzs9rxx6",
                PublicKey = "7793g3g2d5wnv895",
                PrivateKey = "5f70cad59e17666d2f18786c0d270819"
            };

            return newGateway;
        }

        public IBraintreeGateway GetGateway()
        {
            return CreateGateway();
        }
    }
}
