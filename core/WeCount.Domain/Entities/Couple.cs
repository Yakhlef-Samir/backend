using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WeCount.Domain.Entities
{
    public class Couple
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;

        public DateTime CreatedAt { get; set; }
        // public List<CoupleUser> Members { get; set; }
    }
}