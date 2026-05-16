using FitCore.Application.Interfaces.Contexts;
using FitCore.Application.Interfaces.FacadPatterns;
//using FitCore.Application.Interfaces.Facads;
using FitCore.Application.Services.Members.Commands;
using FitCore.Application.Services.Member.Queries;
//using FitCore.Application.Services.Member.Commands.AddNewMemb;
//using FitCore.Application.Services.Member.Queries.GetMembers;

namespace FitCore.Application.Services.Facads
{
    public class MemberFacad : IMemberFacad
    {
        private readonly IDataBaseContext _context;

        public MemberFacad(
            IDataBaseContext context)
        {
            _context = context;
        }

        private IAddNewMemberService _addNewMemberService;

        public IAddNewMemberService AddNewMemberService
        {
            get
            {
                return _addNewMemberService =
                    _addNewMemberService ??
                    new AddNewMemberService(_context);
            }
        }

        private IGetMembersService _getMembersService;

        public IGetMembersService GetMembersService
        {
            get
            {
                return _getMembersService =
                    _getMembersService ??
                    new GetMembersService(_context);
            }
        }
    }
}