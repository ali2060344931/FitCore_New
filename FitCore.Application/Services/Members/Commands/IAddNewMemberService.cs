using FitCore.Application.Contexts;
using FitCore.Common.Dto;
using FitCore.Domain.Entities.Members;

using System;

using MemberEntity = FitCore.Domain.Entities.Members.Member;

namespace FitCore.Application.Services.Members.Commands
{
    public interface IAddNewMemberService
    {
        ResultDto Execute(RequestAddNewMemberDto request);
    }

    public class AddNewMemberService : IAddNewMemberService
    {
        private readonly IDataBaseContext _context;

        public AddNewMemberService(IDataBaseContext context)
        {
            _context = context;
        }

        public ResultDto Execute(RequestAddNewMemberDto request)
        {
            MemberEntity member = new MemberEntity()
            {
                GymId = request.GymId,

                FirstName = request.FirstName,

                LastName = request.LastName,

                Mobile = request.Mobile,

                Gender = request.Gender,

                BirthDate = request.BirthDate,

                IsActive = true
            };

            _context.Members.Add(member);

            _context.SaveChanges();

            return new ResultDto()
            {
                IsSuccess = true,
                Message = "عضو جدید با موفقیت ثبت شد"
            };
        }
    }

    public class RequestAddNewMemberDto
    {
        public long GymId { get; set; }

        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Mobile { get; set; }

        public Gender Gender { get; set; }

        public string BirthDate { get; set; }
    }
}
