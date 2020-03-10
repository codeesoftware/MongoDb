using MongoDb.Entities;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace MongoDb
{
    class MyMongoDbClient
    {
        private const int operationCount = 1000000;
        private const int operationLocationCount = 1;
        private const int operationDetailCount = 20;
        private const string databaseName = "Poc";
        private const string collectionName = "Inventory";
        private readonly MongoClient client;
        private readonly string mongoDBConnectionString = "mongodb+srv://admin:Fireflower20@cluster0-emlrs.mongodb.net/test?retryWrites=true&w=majority";

        public MyMongoDbClient()
        {
            client = new MongoClient(mongoDBConnectionString);
        }
        internal void Insert()
        {
            var database = client.GetDatabase(databaseName);
            //    var t = database.RunCommand(new ObjectCommand<string>(();
            var bigDataCollection = database.GetCollection<Operation>(collectionName);
            var result = new List<Operation>(operationCount);
            for (int i = 0; i < operationCount; i++)
            {
                var operation = new Operation
                {
                    Id = Guid.NewGuid(),
                    Comment = $"{i}",
                    CreatedOn = DateTime.Now,
                    OperationState = Beezie.Domain.Enums.OperationStates.Finalized,
                    OperationType = i % 1000 == 0 ? Beezie.Domain.Enums.OperationTypes.Inventory : Beezie.Domain.Enums.OperationTypes.Out,

                };
                var operationLocations = new List<OperationLocation>();

                for (int j = 0; j < operationLocationCount; j++)
                {
                    var operationLocation = new OperationLocation
                    {
                        Id = Guid.NewGuid(),
                        OperationLocationType = operation.OperationType == Beezie.Domain.Enums.OperationTypes.Inventory
                        ? Beezie.Domain.Enums.OperationLocationTypes.Inventory
                        : Beezie.Domain.Enums.OperationLocationTypes.Sale,
                        StorageId = 42,
                    };

                    var operationDetails = new List<OperationDetail>();
                    for (int k = 0; k < operationDetailCount; k++)
                    {
                        var operationDetail = new OperationDetail
                        {
                            Id = Guid.NewGuid(),
                            CreatedOn = DateTime.Now,
                            FinalizedOn = DateTime.Now.AddMinutes(5),
                            PaidOn = DateTime.Now.AddMinutes(5),
                            PriceId = 42,
                            ProductLocationId = k,
                            Quantity = 42 * k + 1
                        };
                        operationDetails.Add(operationDetail);
                    }
                    operationLocation.OperationDetails = operationDetails;
                    operationLocations.Add(operationLocation);
                }

                operation.OperationLocations = operationLocations;
                result.Add(operation);
            }
            bigDataCollection.InsertMany(result);

            Console.WriteLine($"insert {operationCount}*{operationLocationCount}*{operationDetailCount} = {operationCount * operationLocationCount * operationDetailCount}");

        }

        internal void Query()
        {

            var database = client.GetDatabase(databaseName);
            var bigDataCollection = database.GetCollection<Operation>(collectionName);

            var keyC = Builders<Operation>.IndexKeys.Ascending("CreatedOn");
            var keyO = Builders<Operation>.IndexKeys.Ascending("OperationState");
            bigDataCollection.Indexes.CreateOne(new CreateIndexModel<Operation>(keyC));
            bigDataCollection.Indexes.CreateOne(new CreateIndexModel<Operation>(keyO));

            var filter = Builders<OperationDetail>.Filter.Eq("Quantity", 2);

            Stopwatch sw = Stopwatch.StartNew();
            var latestInventory = bigDataCollection
                .Find<Operation>(d => d.OperationState == Beezie.Domain.Enums.OperationStates.Finalized && d.OperationType == Beezie.Domain.Enums.OperationTypes.Inventory)
                .SortByDescending(o => o.CreatedOn)
                .Limit(1).First();

            var stock = bigDataCollection
              .Find<Operation>(d => d.OperationState == Beezie.Domain.Enums.OperationStates.Finalized && latestInventory.CreatedOn <= d.CreatedOn)
              .ToList();

            sw.Stop();
            Console.WriteLine($"query {sw.Elapsed} s");

        }

    }
}
