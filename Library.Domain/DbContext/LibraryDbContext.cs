using Library.Domain.Models;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.DbContext
{
    public class LibraryDbContext : IDbContext
    {
        public IMongoDatabase Database { get; set; }
        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return Database.GetCollection<T>(collectionName);
        }



        public  LibraryDbContext (MongoClientOptions Options)
        {
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(Options.ConnectionString));
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };

            var client = new MongoClient(settings);
            Database = client.GetDatabase(Options.DataBaseName);
        }

    }
}
