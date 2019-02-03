using System;

namespace XDelivered.StarterKits.NgCoreCosmosDb.Modals
{
    public class LoginResponse
    {
        public DateTime Expiration { get; set; }
        public string Token { get; set; }
    }
}