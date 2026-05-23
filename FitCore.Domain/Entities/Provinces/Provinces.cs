using FitCore.Domain.Entities.Gyms;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitCore.Domain.Entities.Provinces
{
    public class Provinces
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<Cities> Ciltys { get; set; }
    }

    public class Cities
    {
        public int Id { get; set; }
        public int ProvincesId { get; set; }
        public string Name { get; set; }
        public virtual Provinces Provinces { get; set; }

        public virtual ICollection<Gyms.Gyms> Gyms { get; set; }
    }
}
