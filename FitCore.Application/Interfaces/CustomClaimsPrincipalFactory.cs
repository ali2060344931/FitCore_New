using FitCore.Domain.Entities.Users;

using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

using System.Security.Claims;
using System.Threading.Tasks;

namespace FitCore.Application.Interfaces
{
    public class CustomClaimsPrincipalFactory
        : UserClaimsPrincipalFactory<AppUser, IdentityRole<long>>
    {
        public CustomClaimsPrincipalFactory(
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole<long>> roleManager,
            IOptions<IdentityOptions> options)
            : base(userManager, roleManager, options)
        {
        }

        protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AppUser user)
        {
            var identity = await base.GenerateClaimsAsync(user);

            // ✅ نام واقعی کاربر
            identity.AddClaim(new Claim(ClaimTypes.Name, user.FullName ?? ""));

            // ✅ اگر GymId لازم داری
            identity.AddClaim(new Claim("GymId", user.GymId.ToString()));

            return identity;
        }
    }
}
