using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace XDelivered.StarterKits.NgCoreCosmosDb.Helpers
{
    public static class RoleHelper
    {
        public static async Task AddRoleForUser(this UserManager<IdentityUser> userManager, IdentityUser user, Roles modelRole)
        {
            await userManager.AddToRoleAsync(user, modelRole.ToString());
        }
    }

    public enum Roles
    {
        User = 0,
        Owner = 1,
        Admin = 2,
    }
}