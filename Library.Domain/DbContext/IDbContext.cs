using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.DbContext
{
   public interface IDbContext
    {
        IMongoDatabase Database { get; set; }
        IMongoClient MongoClient { get; set; }
    }
}
