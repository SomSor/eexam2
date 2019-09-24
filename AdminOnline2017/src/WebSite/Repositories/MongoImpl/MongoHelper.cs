using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebSite.Repositories.MongoImpl
{
    public class MongoHelper
    {
        private MongoClient client;

        public MongoHelper(MongoClient client)
        {
            this.client = client;
        }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            var db = client.GetDatabase("dsd");
            return db.GetCollection<T>(collectionName);
        }
    }
}
