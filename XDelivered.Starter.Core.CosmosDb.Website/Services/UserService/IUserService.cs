using System.Collections.Generic;
using System.Threading.Tasks;
using XDelivered.StarterKits.NgCoreCosmosDb.Data;
using XDelivered.StarterKits.NgCoreCosmosDb.Modals;

namespace XDelivered.StarterKits.NgCoreCosmosDb.Services
{
    public interface IUserService
    {
        Task<List<UserModel>> GetAllUsers();
        Task DeleteUser(User user);
        Task EditUser(UserModel userModel);
        Task<UserModel> GetUser(string id);
    }
}