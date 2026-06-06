using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Members.Queries
{
    public interface IGetMemberBodyMeasurementByMemberIdService
    {
        List<GetMemberBodyMeasurementDto> Execute(long memberId);
    }
}
