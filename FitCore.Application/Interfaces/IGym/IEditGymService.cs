using FitCore.Application.Services.Gyms.Commands.EditGym;
using FitCore.Application.ViewModels.Gyms;
using FitCore.Common.Dto;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Interfaces.IGym
{
    public interface IEditGymService
    {
        //void Update(UpdateGymDto dto);

        ResultDto Execute(UpdateGymDto dto);
    }
}
