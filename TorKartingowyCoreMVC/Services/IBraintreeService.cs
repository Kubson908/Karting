using Braintree;

namespace TorKartingowyCoreMVC.Services
{
    public interface IBraintreeService
    {
        IBraintreeGateway CreatedGateway();
        IBraintreeGateway GetGateway();
    }
}
