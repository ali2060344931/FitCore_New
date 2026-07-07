using FitCore.Application.Contexts;
using FitCore.Application.FacadPatterns;
using FitCore.Application.Services.Member.Queries;
using FitCore.Application.Services.Members.Commands;
using FitCore.Domain.Entities.Users;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace FitCore.Application.Services.Facads
{
    public class MemberFacad : IMemberFacad
    {

        private readonly IDataBaseContext _context;

        private readonly UserManager<AppUser> _userManager;

        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IFileCompressionService _fileService;


        public MemberFacad(
            IDataBaseContext context,
            UserManager<AppUser> userManager,
            IHttpContextAccessor httpContextAccessor,
            IFileCompressionService fileCompressionService)
        {
            _context = context;

            _userManager = userManager;

            _httpContextAccessor =
                httpContextAccessor;
            _fileService = fileCompressionService;
        }

        private IAddNewMemberService _addNewMemberService;

        public IAddNewMemberService AddNewMemberService
        {
            get
            {
                return _addNewMemberService =
                    _addNewMemberService ??
                    new AddNewMemberService(
                        _context,
                        _userManager, _fileService);
            }
        }



        private IGetMembersService _getMembersService;

        public IGetMembersService GetMembersService
        {
            get
            {
                return _getMembersService =
                    _getMembersService ??
                    new GetMembersService(
                        _context,
                        _httpContextAccessor);
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