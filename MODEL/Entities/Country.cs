using System;
using System.Collections.Generic;

namespace MODEL.Entities
{
    public partial class Country
    {
        public Country()
        {
            Cities = new HashSet<City>();
        }

        public string Id { get; set; } = null!;
        public DateOnly Created { get; set; }
        public DateOnly? Modified { get; set; }
        public bool IsDeleted { get; set; }
        public string Name { get; set; } = null!;

        public virtual ICollection<City> Cities { get; set; }
    }
}
