using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Members.Queries
{
    public class ResultGetMemberBodyMeasurementsDto
    {
        public List<GetMemberBodyMeasurementDto> Measurements { get; set; }

        public int CurrentPage { get; set; }

        public int PageCount { get; set; }

        public int RowCount { get; set; }

        public int PageSize { get; set; }
    }
}
