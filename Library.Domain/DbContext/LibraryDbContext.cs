using Library.Domain.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.DbContext
{
    public class LibraryDbContext : IDbContext
    {
        private readonly ILogger<LibraryDbContext> logger;

        public IMongoDatabase Database { get; set; }
        public IMongoClient MongoClient { get; set; }

        public IMongoCollection<T> GetCollection<T>(string collectionName)
        {
            return Database.GetCollection<T>(collectionName);
        }



        public LibraryDbContext(MongoClientOptions Options, ILogger<LibraryDbContext> logger)
        {
            MongoClientSettings settings = MongoClientSettings.FromUrl(new MongoUrl(Options.ConnectionString));
            //settings.SslSettings = new SslSettings() { EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12 };

            var client = new MongoClient(settings);
            MongoClient = client;
            Database = client.GetDatabase(Options.DataBaseName);
            this.logger = logger;
            if (Database is null)
            {
                logger.LogInformation("Failed to connect to mongo database");
            }
            else
            {
                logger.LogInformation("Mongo connection established from docker");

            }
        }

    }
}
