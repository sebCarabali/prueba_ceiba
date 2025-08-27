using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace bgtpactual.Models
{
    public class SubscribedFund
    {
        [BsonRepresentation(BsonType.ObjectId)]
        public string FundId { get; set; }
        [BsonElement("nombreFondo")]
        public string FundName { get; set; }
        [BsonElement("cantidadInvertida")]
        public decimal SubscriptionAmount { get; set; }
        [BsonElement("fecha")]
        public DateTime SubscriptionDate { get; set; }
        public string ComunicationChannel { get; set; } = "Email"; // sms, email
    }
}
