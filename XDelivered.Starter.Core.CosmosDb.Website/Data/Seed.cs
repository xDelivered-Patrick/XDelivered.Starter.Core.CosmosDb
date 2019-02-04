using System;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Identity.MongoDB;
using Microsoft.AspNetCore.Identity;
using XDelivered.StarterKits.NgCoreCosmosDb.Helpers;

namespace XDelivered.StarterKits.NgCoreCosmosDb.Data
{
    public static class Seed
    {
        private static User _user;
        private static User _owner;
        private static User _admin;
        private static User _user2;

        public static async Task SeedDb(MongoUserStore<User> xdb, UserManager<User> userManager)
        {
            await Reset(xdb);
            await SeedUsers(userManager);
            await SeedData(xdb);
        }

        private static async Task Reset(MongoUserStore<User> xdb)
        {

        }

        private static async Task SeedData(MongoUserStore<User> xdb)
        {

        }
        
        private static async Task SeedUsers(UserManager<User> userManager)
        {
            if (!userManager.Users.Any(x => x.Email.Value == "standard@xdelivered.com"))
            {
                //standard user
                _user = new User("Mr. UserMoor", "standard@xdelivered.com")
                {
                    Created = DateTime.UtcNow,
                };
                await userManager.CreateAsync(_user, "xdelivered99");
                //await userManager.AddToRoleAsync(_user, Roles.User.ToString());
            }
            else
            {
                _user = userManager.Users.FirstOrDefault(x => x.Email.Value == "standard@xdelivered.com");
            }

            if (!userManager.Users.Any(x => x.Email.Value == "standard2@xdelivered.com"))
            {
                _user2 = new User("User 2", "standard2@xdelivered.com")
                {
                    Name = "User 2",
                    Created = DateTime.UtcNow
                };
                await userManager.CreateAsync(_user2, "xdelivered99");
                //await userManager.AddToRoleAsync(_user2, Roles.User.ToString());
            }
            else
            {
                _user2 = userManager.Users.FirstOrDefault(x => x.Email.Value == "standard2@xdelivered.com");
            }
        }
    }
}
