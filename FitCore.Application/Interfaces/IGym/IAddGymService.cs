using FitCore.Application.Services.Gyms.Commands.AddGym;
using FitCore.Application.Services.Members.Commands;
using FitCore.Application.ViewModels.Gyms;
using FitCore.Common.Dto;

using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
namespace FitCore.Application.Interfaces.IGym
{
    public interface IAddGymService
    {
        Task<ResultDto> Execute(CreateGymDto request);
    }

}
