using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace bgtpactual.Models
{
    public class Transaction
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("idFondo")]
        public string? FundId { get; set; }

        [BsonElement("nombreFondo")]
        public string? FundName { get; set; }

        [BsonElement("cantidad")]
        public decimal Amount { get; set; }

        [BsonElement("fecha")]
        public DateTime Date { get; set; }

        [BsonElement("tipo")]
        public string? Type { get; set; }
    }
}
