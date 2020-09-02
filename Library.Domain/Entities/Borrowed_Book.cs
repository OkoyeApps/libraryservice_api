using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.Entities
{
   public  class Borrowed_Book : BaseEntity
    {
        public ObjectId Book { get; set; }
        public ObjectId User { get; set; }
        public bool Returned { get; set; } = false;
    }
}
