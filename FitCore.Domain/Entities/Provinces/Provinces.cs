using System.Collections.Generic;

namespace FitCore.Domain.Entities.Provinces
{
    public class Province
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual ICollection<City> Ciltys { get; set; }
    }

    public class City
    {
        public int Id { get; set; }
        public int ProvincesId { get; set; }
        public string Name { get; set; }
        public virtual Province Provinces { get; set; }

        public virtual ICollection<Gyms.Gyms> Gyms { get; set; }
    }
}
