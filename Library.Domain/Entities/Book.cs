using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Threading.Tasks;

namespace Library.Domain.Entities
{
   public  class Book : BaseEntity
    {
        public Book()
        {
            //var now = DateTime.UtcNow;
            //this.Id = ObjectId.GenerateNewId(now);
        }
        //[BsonId]
        //public ObjectId Id { get; private set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string TotalPages { get; set; }
        public long Total_Copies_Available { get; set; }
        public ObjectId Author { get; set; } //this is the author id as a reference here
        public string Isbn { get; set; }
        public string Publisher { get; set; }
        public long Copies_on_Rent { get; set; }
    }
}
