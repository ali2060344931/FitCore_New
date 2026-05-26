using FitCore.Application.Services.Gyms.Commands.AddGym;
using FitCore.Application.Services.Member.Queries;
using FitCore.Common.Dto;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Interfaces.IMembers
{
    public interface IGetMembersByIdService
    {
        ResultDto<GetMemberByIdDto> Execute(int Id);
    }
}
