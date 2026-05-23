using FitCore.Application.Services.Members.Commands;
using FitCore.Application.Services.Member.Queries;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.FacadPatterns
{
    public interface IMemberFacad
    {
        IAddNewMemberService AddNewMemberService { get; }

        IEditMemberService EditMemberService { get; }

        IRemoveMemberService RemoveMemberService { get; }

        IGetMembersService GetMembersService { get; }
    }
}
