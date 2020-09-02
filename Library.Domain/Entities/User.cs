using MongoDB.Bson;
using System.Collections.Generic;
using System.Dynamic;

namespace Library.Domain.Entities
{
    public class User : BaseEntity
    {
        public User() : base()
        {
            this.Claims = new BsonArray();
        }
        public string Username { get; set; }
        public string Password { get; set; }
        public BsonArray Claims { get; set; }
    }
}
