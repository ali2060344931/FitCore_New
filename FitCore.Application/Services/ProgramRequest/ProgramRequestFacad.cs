using FitCore.Application.Contexts;
using FitCore.Application.Services.ProgramRequests;

namespace FitCore.Application.FacadPatterns
{
    public interface IProgramRequestFacad
    {
        ISubmitProgramRequestService SubmitProgramRequestService { get; }
        IUpdateProgramRequestService UpdateProgramRequestService { get; }
        IGetProgramRequestsForAdminService GetProgramRequestsForAdminService { get; }
        IGetProgramRequestsForMemberService GetProgramRequestsForMemberService { get; }
        IGetPendingRequestsCountService GetPendingRequestsCountService { get; }
    }

    public class ProgramRequestFacad : IProgramRequestFacad
    {
        private readonly IDataBaseContext _context;

        public ProgramRequestFacad(IDataBaseContext context)
        {
            _context = context;
        }

        private ISubmitProgramRequestService _submit;
        public ISubmitProgramRequestService SubmitProgramRequestService =>
            _submit ??= new SubmitProgramRequestService(_context);

        private IUpdateProgramRequestService _update;
        public IUpdateProgramRequestService UpdateProgramRequestService =>
            _update ??= new UpdateProgramRequestService(_context);

        private IGetProgramRequestsForAdminService _getForAdmin;
        public IGetProgramRequestsForAdminService GetProgramRequestsForAdminService =>
            _getForAdmin ??= new GetProgramRequestsForAdminService(_context);

        private IGetProgramRequestsForMemberService _getForMember;
        public IGetProgramRequestsForMemberService GetProgramRequestsForMemberService =>
            _getForMember ??= new GetProgramRequestsForMemberService(_context);

        private IGetPendingRequestsCountService _getCount;
        public IGetPendingRequestsCountService GetPendingRequestsCountService =>
            _getCount ??= new GetPendingRequestsCountService(_context);
    }
}
