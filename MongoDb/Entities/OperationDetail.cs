using MongoDB.Bson.Serialization.Attributes;
using System;

namespace MongoDb.Entities
{
    public class OperationDetail 
    {
        [BsonId]
        public Guid Id { get; set; }

        [BsonElement(nameof(Quantity))]
        public decimal? Quantity { get; set; }
        [BsonElement(nameof(ProductLocationId))]
        public int ProductLocationId { get; set; }
        [BsonElement(nameof(CreatedOn))]
        public DateTime CreatedOn { get; set; }
        [BsonElement(nameof(FinalizedOn))]
        public DateTime? FinalizedOn { get; set; }
        [BsonElement(nameof(PaidOn))]
        public DateTime? PaidOn { get; set; }
        [BsonElement(nameof(PriceId))]
        public int? PriceId { get; set; }
    }
}
