using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Swashbuckle.Swagger.Annotations;
using XDelivered.StarterKits.NgCoreCosmosDb.Data;
using XDelivered.StarterKits.NgCoreCosmosDb.Exceptions;
using XDelivered.StarterKits.NgCoreCosmosDb.Helpers;
using XDelivered.StarterKits.NgCoreCosmosDb.Modals;
using XDelivered.StarterKits.NgCoreCosmosDb.Settings;
using ControllerBase = XDelivered.StarterKits.NgCoreCosmosDb.Helpers.ControllerBase;
using IdentityResult = Microsoft.AspNetCore.Identity.IdentityResult;
using PasswordVerificationResult = Microsoft.AspNetCore.Identity.PasswordVerificationResult;

namespace XDelivered.StarterKits.NgCoreCosmosDb.Controllers
{
    [Route("api/account")]
    [ApiController]
    [Produces("application/json")]
    public class AccountController : Helpers.ControllerBase
    {
        private readonly Microsoft.AspNetCore.Identity.UserManager<User> _userManager;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IOptions<AppConfiguration> _config;

        public AccountController(UserManager<User> userManager,
            IPasswordHasher<User> passwordHasher,
            IOptions<AppConfiguration> config)
        {
            _userManager = userManager;
            _passwordHasher = passwordHasher;
            _config = config;
        }

        [Route("register")]
        [HttpPost]
        [Produces(typeof(OperationResult))]
        [SwaggerOperation("Register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestModel requestModel)
        {
            var user = new User(requestModel.Name, requestModel.Email)
            {
                Name = requestModel.Name,
                Created = DateTime.UtcNow
            };
            IdentityResult createUserResult = await _userManager.CreateAsync(user, requestModel.Password);

            await _userManager.AddToRoleAsync(user, requestModel.Role.ToString());

            if (!createUserResult.Succeeded)
            {
                throw new UserMessageException(createUserResult.Errors.First().Description);
            }

            return Ok();
        }
        
        [HttpPost("login")]
        [Produces(typeof(OperationResult<LoginResponse>))]
        [SwaggerOperation(nameof(Login))]
        public async Task<ActionResult> Login([FromBody] LoginRequestModel requestModel)
        {
            User user = await _userManager.FindByEmailAsync(requestModel.Email);

            if (user == null || _passwordHasher.VerifyHashedPassword(user, user.PasswordHash, requestModel.Password) != PasswordVerificationResult.Success || user.Deleted)
            {
                return Unauthorized();
            }

            var token = await JwtTokenHelper.GetJwtSecurityToken(user, _userManager, _config);

            return Json(new LoginResponse()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = token.ValidTo
            });
        }

        [HttpGet("info")]
        [Authorize]
        [Produces(typeof(OperationResult<UserInfoResponseModel>))]
        [SwaggerOperation(nameof(GetUserInfo))]
        public async Task<UserInfoResponseModel> GetUserInfo()
        {
            var userId = base.UserId;
            User user = await _userManager.FindByIdAsync(userId);
            IList<string> roles = await _userManager.GetRolesAsync(user);

            return new UserInfoResponseModel()
            {
                Email = user.Email.NormalizedValue,
                Created = user.Created,
                Role = roles.FirstOrDefault()
            };
        }
    }
}