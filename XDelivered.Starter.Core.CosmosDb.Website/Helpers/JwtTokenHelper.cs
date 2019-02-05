using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using XDelivered.StarterKits.NgCoreCosmosDb.Data;
using XDelivered.StarterKits.NgCoreCosmosDb.Settings;

namespace XDelivered.StarterKits.NgCoreCosmosDb.Helpers
{
    public static class JwtTokenHelper
    {
        public static async Task<JwtSecurityToken> GetJwtSecurityToken<T>(T user, UserManager<T> userManager,
            IOptions<AppConfiguration> config) where T : User
        {
            var userClaims = await userManager.GetClaimsAsync(user);

            var roles = await userManager.GetRolesAsync(user);

            return new JwtSecurityToken(
                config.Value.SiteUrl,
                config.Value.SiteUrl,
                GetTokenClaims(user, roles).Union(userClaims),
                expires: DateTime.UtcNow.AddDays(config.Value.TokenExpiryDays),
                signingCredentials: new SigningCredentials(
                    new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config.Value.SigningKey)),
                    SecurityAlgorithms.HmacSha256)
            );
        }

        private static IEnumerable<Claim> GetTokenClaims(User user, IList<string> roles)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                new Claim("UserId", user.Id.ToString())
            };

            foreach (var role in roles)
            {
                claims.Add(new Claim("http://schemas.microsoft.com/ws/2008/06/identity/claims/role", role));
            }

            return claims;
        }
    }
}