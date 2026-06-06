using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Application.Services.Members.Queries
{
    public class GetMemberBodyMeasurementDto
    {
        public long Id { get; set; }
        public long MemberId { get; set; }
        public string RecordDate { get; set; }

        public decimal? Weight { get; set; }
        public decimal? Height { get; set; }
        public decimal? BodyFatPercentage { get; set; }
        public decimal? Waist { get; set; }
        public decimal? Hip { get; set; }
        public decimal? Chest { get; set; }
    }
}
