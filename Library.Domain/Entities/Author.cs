using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using MongoDB.Bson.Serialization.Attributes;

namespace Library.Domain.Entities
{
    public class Author
    {
        public Author()
        {
            var now = DateTime.UtcNow;
            this.Id = ObjectId.GenerateNewId(now);
        }
        [BsonId]
        public ObjectId Id { get; private set; }
        public string Full_Name { get; set; }
        public string Publisher { get; set; }
        public string Author_Bio { get; set; }
        public string Avatar_Url { get; set; }

    }
}
