using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.DBQueries
{
    public class AuthorQueries
    {
        public BsonDocument[] GenerateQueryForAutherAndBooks(ObjectId AuthorId)
        {
            var match = new BsonDocument
            {
                {
                    "$match", new BsonDocument
                    {
                        {
                            "_id", new BsonDocument {
                                {"$eq", AuthorId }
                            }
                        }
                    }
                }
            };

            var lookUp = new BsonDocument
            {
                {
                    "$lookup", new  BsonDocument {
                        { "from", "Books" },
                        { "localField" , "_id" },
                        {"foreignField",  "Author" },
                        {"as" , "Author_Books" }
                    }
                }
            };

            return new[] { match, lookUp };
        }
    }
}
