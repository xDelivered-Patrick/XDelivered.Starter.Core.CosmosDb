using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using xDelivered.Common;
using XDelivered.StarterKits.NgCoreCosmosDb.Data;
using XDelivered.StarterKits.NgCoreCosmosDb.Exceptions;
using XDelivered.StarterKits.NgCoreCosmosDb.Helpers;
using XDelivered.StarterKits.NgCoreCosmosDb.Modals;

namespace XDelivered.StarterKits.NgCoreCosmosDb.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly UserManager<User> _aspNetUserManager;

        public UserService(ApplicationDbContext dbContext, UserManager<User> aspNetUserManager)
        {
            _dbContext = dbContext;
            _aspNetUserManager = aspNetUserManager;
        }

        public async Task<List<UserModel>> GetAllUsers()
        {
            var users = await _dbContext.Users.Where(x=>!x.Deleted).ToListAsync();
            return users.Select(x=>Map(x)).ToList();
        }

        public async Task DeleteUser(User user)
        {
            if (user == null)
            {
                throw new UserMessageException("User could not be found");
            }

            user = _dbContext.Users.Single(x => x.Id == user.Id);
            user.Deleted = true;
            await _dbContext.SaveChangesAsync();
        }
        
        public async Task EditUser(UserModel userModel)
        {
            var user = await _aspNetUserManager.FindByIdAsync(userModel.Id);

            if (user == null)
            {
                throw new UserMessageException("User not found");
            }

            IList<string> roles = await _aspNetUserManager.GetRolesAsync(user);
            var currentRole = roles.First();

            //role change?
            var roleString = ((Roles)int.Parse(userModel.Role)).ToString();
            if (currentRole != roleString)
            {
                await _aspNetUserManager.RemoveFromRoleAsync(user, currentRole);
                await _aspNetUserManager.AddToRoleAsync(user, roleString);
            }

            //password change
            if (userModel.Password.IsNotNullOrEmpty())
            {
                await _aspNetUserManager.RemovePasswordAsync(user);
                await _aspNetUserManager.AddPasswordAsync(user, userModel.Password);
            }

            user.Name = userModel.Name;
            user.Email = userModel.Email;
            user.UserName = userModel.Email;

            await _aspNetUserManager.UpdateAsync(user);
        }

        public async Task<UserModel> GetUser(string id)
        {
            var user = _dbContext.Users.SingleOrDefault(x => x.Id == id);

            if (user == null)
            {
                throw new UserMessageException("User not found");
            }

            IList<string> roles = await _aspNetUserManager.GetRolesAsync(user);
            var role = roles.First();

            return Map(user, role);
        }

        private UserModel Map(User user, string role = null)
        {
            return new UserModel()
            {
                Id = user.Id,
                Email = user.Email,
                Name = user.Name,
                Role = role
            };
        }
    }
}
