using FitCore.Application.Services.Gyms.Commands.AddGym;

using System.Collections.Generic;

namespace FitCore.Application.Interfaces.IGym
{
    public interface IGetGymsService
    {
        List<GymDto> GetAll();
    }
}