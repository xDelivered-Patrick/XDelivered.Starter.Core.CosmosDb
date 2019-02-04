using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetCore.Identity.MongoDB;
using AspNetCore.Identity.MongoDB.Models;
using Microsoft.AspNetCore.Identity;
using MongoDB.Driver;
using xDelivered.Common;
using XDelivered.StarterKits.NgCoreCosmosDb.Data;
using XDelivered.StarterKits.NgCoreCosmosDb.Exceptions;
using XDelivered.StarterKits.NgCoreCosmosDb.Helpers;
using XDelivered.StarterKits.NgCoreCosmosDb.Modals;

namespace XDelivered.StarterKits.NgCoreCosmosDb.Services
{
    public class UserService : IUserService
    {
        private readonly IMongoCollection<User> _xdb;
        private readonly UserManager<User> _aspNetUserManager;

        public UserService(IMongoCollection<User> xdb, UserManager<User> aspNetUserManager)
        {
            _xdb = xdb;
            _aspNetUserManager = aspNetUserManager;
        }

        public async Task<List<UserModel>> GetAllUsers()
        {
            List<User> users = await _xdb.AsQueryable().ToListAsync();
            return users.Select(x=>Map(x)).ToList();
        }

        public async Task DeleteUser(User user)
        {
            if (user == null)
            {
                throw new UserMessageException("User could not be found");
            }

            await _xdb.DeleteOneAsync(x=>x.Id == user.Id);
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
            user.Email.SetNormalizedEmail(userModel.Email);

            await _aspNetUserManager.UpdateAsync(user);
        }

        public async Task<UserModel> GetUser(string id)
        {
            var user = await _xdb.Find(x => x.Id == id).FirstOrDefaultAsync();
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
                Email = user.Email.Value,
                Name = user.Name,
                Role = role
            };
        }
    }
}
