using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Pages.Account.Manage.Internal;
using XDelivered.StarterKits.NgCoreCosmosDb.Helpers;

namespace XDelivered.StarterKits.NgCoreCosmosDb.Data
{
    public static class Seed
    {
        private static User _user;
        private static User _owner;
        private static User _admin;
        private static User _user2;

        public static async Task SeedDb(ApplicationDbContext dbContext, UserManager<User> userManager)
        {
            await Reset(dbContext);
            await SeedUsers(userManager);
            await SeedData(dbContext);
        }

        private static async Task Reset(ApplicationDbContext dbContext)
        {
            await dbContext.SaveChangesAsync();
        }

        private static async Task SeedData(ApplicationDbContext dbContext)
        {
            

            await dbContext.SaveChangesAsync();
        }
        
        private static async Task SeedUsers(UserManager<User> userManager)
        {
            if (!userManager.Users.Any(x => x.Email == "standard@xdelivered.com"))
            {
                //standard user
                _user = new User()
                {
                    Name = "Mr. UserMoor",
                    Email = "standard@xdelivered.com",
                    UserName = "standard@xdelivered.com",
                    Created = DateTime.UtcNow,
                };
                await userManager.CreateAsync(_user, "xdelivered99");
                await userManager.AddToRoleAsync(_user, Roles.User.ToString());
            }
            else
            {
                _user = userManager.Users.FirstOrDefault(x => x.Email == "standard@xdelivered.com");
            }

            if (!userManager.Users.Any(x => x.Email == "standard2@xdelivered.com"))
            {
                _user2 = new User()
                {
                    Name = "User 2",
                    Email = "standard2@xdelivered.com",
                    UserName = "standard2@xdelivered.com",
                    Created = DateTime.UtcNow
                };
                await userManager.CreateAsync(_user2, "xdelivered99");
                await userManager.AddToRoleAsync(_user2, Roles.User.ToString());
            }
            else
            {
                _user2 = userManager.Users.FirstOrDefault(x => x.Email == "standard2@xdelivered.com");
            }

            //owner user
            if (!userManager.Users.Any(x => x.Email == "owner@xdelivered.com"))
            {
                _owner = new User()
                {
                    Name = "Mr. OwnerMoor",
                    Email = "owner@xdelivered.com",
                    UserName = "owner@xdelivered.com",
                    Created = DateTime.UtcNow,
                };
                await userManager.CreateAsync(_owner, "xdelivered99");
                await userManager.AddToRoleAsync(_owner, Roles.Owner.ToString());
            }
            else
            {
                _owner = userManager.Users.FirstOrDefault(x => x.Email == "owner@xdelivered.com");
            }

            //admin user
            if (!userManager.Users.Any(x => x.Email == "admin@xdelivered.com"))
            {
                _admin = new User()
                {
                    Name = "Mr. AdminMoor",
                    Email = "admin@xdelivered.com",
                    UserName = "admin@xdelivered.com",
                    Created = DateTime.UtcNow,
                };
                await userManager.CreateAsync(_admin, "xdelivered99");
                await userManager.AddToRoleAsync(_admin, Roles.Admin.ToString());
            }
            else
            {
                _admin = userManager.Users.FirstOrDefault(x => x.Email == "admin@xdelivered.com");
            }
        }
    }
}
