using FitCore.Domain.Entities.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using FitCore.Application.Interfaces.Contexts;

public class CustomClaimsPrincipalFactory
    : UserClaimsPrincipalFactory<AppUser, IdentityRole<long>>
{
    private readonly IDataBaseContext _context;

    public CustomClaimsPrincipalFactory(
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole<long>> roleManager,
        IOptions<IdentityOptions> options,
        IDataBaseContext context)
        : base(userManager, roleManager, options)
    {
        _context = context;
    }

    protected override async Task<ClaimsIdentity> GenerateClaimsAsync(AppUser user)
    {
        var identity = await base.GenerateClaimsAsync(user);

        // حذف Name پیش‌فرض (که Username بود)
        var existingNameClaim = identity.FindFirst(ClaimTypes.Name);
        if (existingNameClaim != null)
            identity.RemoveClaim(existingNameClaim);

        // افزودن FullName به عنوان Name
        identity.AddClaim(new Claim(ClaimTypes.Name, user.FullName ?? ""));

        // افزودن GymId
        if (user.GymId!=null)
        {
            identity.AddClaim(new Claim("GymId", user.GymId.ToString()));

            // گرفتن نام باشگاه
            var gym = await _context.Gyms
                .FirstOrDefaultAsync(x => x.Id == user.GymId);

            if (gym != null)
            {
                identity.AddClaim(new Claim("GymName", gym.Name));
            }
        }

        return identity;
    }
}
