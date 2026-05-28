using FitCore.Application.Services.Member.Queries;
using FitCore.Application.Services.Members.Commands;

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
