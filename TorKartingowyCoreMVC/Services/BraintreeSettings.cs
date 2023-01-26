using System;
using System.Collections.Generic;
using System.Text;

namespace TorKartingowyCoreMVC.Services
{
    public class BraintreeSettings
    {
        public string Environment { get; set; }
        public string MerchantId { get; set; }
        public string PublicKey { get; set; }
        public string PrivateKey { get; set; }
    }
}
