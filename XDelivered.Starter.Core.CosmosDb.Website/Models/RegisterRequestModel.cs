using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using XDelivered.StarterKits.NgCoreCosmosDb.Helpers;

namespace XDelivered.StarterKits.NgCoreCosmosDb.Modals
{
    public class RegisterRequestModel
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public Roles Role { get; set; }
    }
}
