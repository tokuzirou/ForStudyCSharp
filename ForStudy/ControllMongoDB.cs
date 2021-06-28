using System;
using System.Collections.Generic;

namespace ForStudy
{
    using MongoDB.Bson;
    using MongoDB.Driver;

    class ControllMongoDB
    {
        internal static void Controll()
        {
            //make a connection to a server.
            MongoClient client = new MongoClient("mongodb://localhost:27017");

            //get a database.
            IMongoDatabase database = client.GetDatabase("unko");

            //get a collection.
            IMongoCollection<BsonDocument> collection = database.GetCollection<BsonDocument>("bar");

            //insert a document.
            BsonDocument document = new BsonDocument
            {
                { "name", "MongoDB" },
                { "type", "Database" },
                { "count", 1 },
                { "info", new BsonDocument
                    {
                        { "x", 203 },
                        { "y", 102 },
                        { "z", 100 }
                    }}
            };
            collection.InsertOne(document);
            Console.WriteLine("追加したドキュメント：" + document.ToString());

            //count a number of documents.
            long count = collection.CountDocuments(new BsonDocument());
            Console.WriteLine("コレクション内のドキュメントの数：" + count.ToString());

            //query the collection.
            //find all collection.
            //List<BsonDocument> documentConfirmed = collection.Find(new BsonDocument()).ToList();
            //documentConfirmed.ForEach(documentX => Console.WriteLine(documentX.ToString()));

            //by filtering
            //FilterDefinition<BsonDocument> filter = Builders<BsonDocument>.Filter.Eq("name", "MongoDB");
            //List<BsonDocument> documentConfirmedFuther = collection.Find(filter).ToList();
            //documentConfirmedFuther.ForEach(X => Console.WriteLine(X.ToString()));

            //delete a document
            //FilterDefinition<BsonDocument> filterToDelete = Builders<BsonDocument>.Filter.Gte<string>("name", "MongoDB");
            //DeleteResult countX = collection.DeleteMany(filterToDelete);
            //Console.WriteLine(countX.DeletedCount);
            collection.Indexes.DropAll();

            //drop a collection.
            database.DropCollection("bar");
            List<BsonDocument> documentConirmed = collection.Find(new BsonDocument()).ToList();
            documentConirmed.ForEach(X => Console.WriteLine("コレクション内のドキュメント：" + X.ToString()));

            //drop a database.
            client.DropDatabase("unko");
        }
    }
}
