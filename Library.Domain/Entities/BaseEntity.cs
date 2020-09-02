using Library.Domain.Extensions;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;


namespace Library.Domain.Entities
{
    public class BaseEntity
    {
        protected DateTime now = DateTime.UtcNow;
        public BaseEntity()
        {
            this.CreatedAt = now.ToTimestamp();
            this.Id = ObjectId.GenerateNewId(now);
        }
        [BsonId]
        public ObjectId Id { get; set; }
        public long CreatedAt { get; set; }
        public long UpdatedAt { get; set; }
    }
}
