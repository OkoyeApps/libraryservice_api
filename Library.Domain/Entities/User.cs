using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.Entities
{
    public class User : BaseEntity
    {
        public User() : base()
        {
            this.Claims = new Dictionary<string, string>();
            this.Id = ObjectId.GenerateNewId(now);
        }
        [BsonId]
        public ObjectId Id { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public IDictionary<string, string> Claims { get; set; }
    }
}
