using System.Collections.Generic;
using MongoDB.Bson.Serialization.Attributes;
using WeCount.Domain.Common;

namespace WeCount.Domain.Entities.Couple
{
    public class Couple : AuditableEntity
    {
        [BsonElement("name")]
        public string Name { get; set; } = string.Empty;

        [BsonElement("members")]
        public List<CoupleUser> Members { get; set; } = new List<CoupleUser>();
    }
}
