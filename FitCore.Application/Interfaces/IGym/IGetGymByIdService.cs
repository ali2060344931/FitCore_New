using FitCore.Application.Services.Gyms.Commands.AddGym;

namespace FitCore.Application.Interfaces.IGym
{
    public interface IGetGymByIdService
    {
        GymDto GetById(string code);
    }
}