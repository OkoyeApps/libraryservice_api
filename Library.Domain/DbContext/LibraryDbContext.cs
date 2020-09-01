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

        public static LibraryDbContext Create(string connectionString, string dbName)
        {
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(connectionString));
            settings.SslSettings = new SslSettings() { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };



            var client = new MongoClient(connectionString);
            var dbcontext = (LibraryDbContext)Activator.CreateInstance(typeof(LibraryDbContext));
            //LibraryDbContext dbcontext = new LibraryDbContext();
            dbcontext.Database = client.GetDatabase(dbName);

            return dbcontext;
        }

    }
}
