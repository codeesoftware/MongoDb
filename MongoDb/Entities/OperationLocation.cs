using Beezie.Domain.Enums;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace MongoDb.Entities
{
    public class OperationLocation 
    {
        public OperationLocation()
        {
            OperationDetails = new List<OperationDetail>();
        }

        [BsonId]

        public Guid Id { get; set; }

        //public int OperationId { get; set; }
        //public virtual Operation Operation { get; set; }

        [BsonElement(nameof(StorageId))]
        public int StorageId { get; set; }

        [BsonElement(nameof(OperationLocationType))]
        public OperationLocationTypes OperationLocationType { get; set; }

        [BsonElement(nameof(OperationDetails))]
        public virtual ICollection<OperationDetail> OperationDetails { get; set; }

    }
}