using FitCore.Application.Interfaces.Contexts;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.Gyms;
using FitCore.Domain.Entities.Users;

using Microsoft.AspNetCore.Identity;

using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

public class RegisterGymService
{
    private readonly IDataBaseContext _context;
    private readonly UserManager<AppUser> _userManager;
    private readonly RoleManager<IdentityRole<long>> _roleManager;

    public RegisterGymService(
        IDataBaseContext context,
        UserManager<AppUser> userManager,
        RoleManager<IdentityRole<long>> roleManager)
    {
        _context = context;
        _userManager = userManager;
        _roleManager = roleManager;
    }

    public async Task<ResultDto> Execute(RegisterGymRequest request, CancellationToken cancellationToken = default)
    {
        await using var transaction = await _context.Database.BeginTransactionAsync(cancellationToken);

        try
        {
            var gym = new Gym
            {
                Name = request.GymName,
                Code = Guid.NewGuid().ToString("N")[..8],
                SubDomain = request.SubDomain,
                IsActive = true
            };

            _context.Gyms.Add(gym);
            await _context.SaveChangesAsync(cancellationToken);

            var admin = new AppUser
            {
                UserName = request.Email,
                Email = request.Email,
                FullName = request.FullName,
                GymId = gym.Id,
                IsActive = true
            };

            var result = await _userManager.CreateAsync(admin, request.Password);

            if (!result.Succeeded)
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));

            if (!await _roleManager.RoleExistsAsync("Admin"))
            {
                var roleResult = await _roleManager.CreateAsync(new IdentityRole<long>("Admin"));

                if (!roleResult.Succeeded)
                    throw new Exception(string.Join(", ", roleResult.Errors.Select(e => e.Description)));
            }

            var addToRoleResult = await _userManager.AddToRoleAsync(admin, "Admin");

            if (!addToRoleResult.Succeeded)
                throw new Exception(string.Join(", ", addToRoleResult.Errors.Select(e => e.Description)));

            await transaction.CommitAsync(cancellationToken);

            return ResultDto.Success();
        }
        catch (Exception ex)
        {
            await transaction.RollbackAsync(cancellationToken);
            return ResultDto.Failure(ex.Message);
        }
    }
}

public class RegisterGymRequest
{
    public string GymName { get; set; }
    public string SubDomain { get; set; }
    public string FullName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
}
