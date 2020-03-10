using Beezie.Domain.Enums;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;

namespace MongoDb.Entities
{
    public class Operation 
    {
        public Operation()
        {
            OperationLocations = new List<OperationLocation>();
        }
        [BsonId]
        public Guid Id { get; set; }

        [BsonElement(nameof(OperationType))]
        public OperationTypes OperationType { get; set; }

        [BsonElement(nameof(OperationState))]
        public OperationStates OperationState { get; set; }

        [BsonElement(nameof(CreatedOn))]
        public DateTime CreatedOn { get; set; }

        [BsonElement(nameof(Comment))]
        public string Comment { get; set; }


        [BsonElement(nameof(OperationLocations))]
        public virtual ICollection<OperationLocation> OperationLocations { get; set; }

    }
}
