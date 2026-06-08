using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Members.Queries
{
    public class RequestGetMemberBodyMeasurementsDto
    {
        public long MemberId { get; set; }

        public string SearchKey { get; set; }

        public int Page { get; set; }

        public int PageSize { get; set; }
    }
}
