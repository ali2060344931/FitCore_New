using FitCore.Domain.Entities.Members;

namespace FitCore.Application.Services.Member.Queries
{
    public class ResultGetMemberDto
    {
        public long Id { get; set; }
        public string FullName { get; set; }
        public string Mobile { get; set; }
        public Gender? Gender { get; set; }
        public string BirthDate { get; set; }

        public int countNutritionProg { get; set; }
    }



}
