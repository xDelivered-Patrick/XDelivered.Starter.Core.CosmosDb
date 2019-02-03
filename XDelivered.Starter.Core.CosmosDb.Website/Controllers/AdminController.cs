using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.Swagger.Annotations;
using XDelivered.StarterKits.NgCoreCosmosDb.Data;
using XDelivered.StarterKits.NgCoreCosmosDb.Modals;
using XDelivered.StarterKits.NgCoreCosmosDb.Services;
using ControllerBase = XDelivered.StarterKits.NgCoreCosmosDb.Helpers.ControllerBase;

namespace XDelivered.StarterKits.NgCoreCosmosDb.Controllers
{
    [Route("api/admin")]
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Produces("application/json")]
    public class AdminController : Helpers.ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly IUserService _userService;

        public AdminController(UserManager<User> userManager, IUserService userService)
        {
            _userManager = userManager;
            _userService = userService;
        }
        
        [HttpGet("users")]
        [Produces(typeof(OperationResult<List<UserModel>>))]
        [SwaggerOperation(nameof(GetAllUsers))]
        public async Task<List<UserModel>> GetAllUsers()
        {
            return await _userService.GetAllUsers();
        }

        [HttpGet("user/{id}")]
        [Produces(typeof(OperationResult<UserModel>))]
        [SwaggerOperation(nameof(GetUserById))]
        public async Task<UserModel> GetUserById(string id)
        {
            return await _userService.GetUser(id);
        }

        [HttpDelete("users/{id}")]
        [Produces(typeof(OperationResult))]
        [SwaggerOperation(nameof(DeleteUser))]
        public async Task DeleteUser(string id)
        {
            await _userService.DeleteUser(await _userManager.FindByIdAsync(id));
        }
        
        [HttpPost("users/{id}")]
        [Produces(typeof(OperationResult))]
        [SwaggerOperation(nameof(UpdateUser))]
        public async Task UpdateUser([FromBody]UserModel model)
        {
            await _userService.EditUser(model);
        }

        
        private async Task<User> GetUser()
        {
            return await _userManager.FindByIdAsync(base.UserId);
        }
    }
}