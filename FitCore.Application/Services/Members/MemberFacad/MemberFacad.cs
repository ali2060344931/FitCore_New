using FitCore.Application.Interfaces.Contexts;
using FitCore.Application.Interfaces.FacadPatterns;
using FitCore.Application.Services.Members.Commands;
using FitCore.Application.Services.Member.Queries;

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

        private IEditMemberService _editMemberService;
        public IEditMemberService EditMemberService
        {
            get
            {
                return _editMemberService =
                    _editMemberService ??
                    new EditMemberService(_context);
            }
        }

        private IRemoveMemberService _removeMemberService;
        public IRemoveMemberService RemoveMemberService
        {
            get
            {
                return _removeMemberService =
                    _removeMemberService ??
                    new RemoveMemberService(_context);
            }
        }
    }
}