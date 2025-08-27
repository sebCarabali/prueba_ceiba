using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace bgtpactual.Models
{

    [BsonIgnoreExtraElements]
    public class Fund
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("nombre")]
        public string? Name { get; set; }

        [BsonElement("montoMinimo")]
        public decimal? MinimumAmount { get; set; }

        [BsonElement("categoria")]
        public string? Category { get; set; }
    }
}
