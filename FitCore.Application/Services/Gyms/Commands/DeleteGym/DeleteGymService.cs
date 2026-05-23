using FitCore.Application.Contexts;
using FitCore.Application.Interfaces.IGym;
using FitCore.Application.Interfaces.ISms;
using FitCore.Common.Dto;

using System.Threading;
using System.Threading.Tasks;
namespace FitCore.Application.Services.Gyms.Commands.DeleteGym
{
    public class DeleteGymService : IDeleteGymService
    {

        private readonly IDataBaseContext _context;

        public DeleteGymService(IDataBaseContext context)
        {
            _context = context;
        }


        public async Task<ResultDto> Execute(long id, CancellationToken cancellationToken = default)
        {
            var gym = _context.Gyms.Find(id);

            if (gym == null)
                return new ResultDto
                {
                    IsSuccess = false,
                    Message = "باشگاهی با این مشخصات یافت نشد."
                };

            _context.Gyms.Remove(gym);
            await _context.SaveChangesAsync(cancellationToken);

            return new ResultDto
            {
                IsSuccess = true,
                Message = "با موفقیت حذف شد."
            };
        }



    }

}
