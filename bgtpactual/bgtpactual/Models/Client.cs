using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace bgtpactual.Models
{
    public class Client
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string? Id { get; set; }

        [BsonElement("nombre")]
        public string? Name { get; set; }
        [BsonElement("balance")]
        public decimal? Balance { get; set; }

        [BsonElement("historialTransacciones")]
        public List<Transaction>? Transactions { get; set; }
        [BsonElement("fondosSuscritos")]
        public List<SubscribedFund>? Funds { get; set; }
    }
}
