using Authentication.Core.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace Authentication_Api.Extensions
{
    public static class UserManagerExtensions
    {
        public static async Task<AppUser> FindUserByClaimsPrincipleWithAddress(this UserManager<AppUser> userManager,
            ClaimsPrincipal user)
        {
            var email =user?.Claims?.FirstOrDefault(x=>x.Type==ClaimTypes.Email)?.Value;
            return await userManager.Users.Include(x => x.address).SingleOrDefaultAsync();
        }

        public static async Task<AppUser> FindEmailByClaimsPrinciple(this UserManager<AppUser> userManager,
            ClaimsPrincipal user)
        {
            var email = user?.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Email)?.Value;
            return await userManager.Users.SingleOrDefaultAsync();
        }
    }
}
