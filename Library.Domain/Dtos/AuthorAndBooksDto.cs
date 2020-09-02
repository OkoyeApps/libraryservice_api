using Library.Domain.Entities;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.Dtos
{
    public class AuthorAndBooksDto
    {
        public AuthorAndBooksDto()
        {
            this.Author_Books = new HashSet<Book>();
        }
        public ObjectId Id { get; private set; }
        public string Full_Name { get; set; }
        public string Publisher { get; set; }
        public string Author_Bio { get; set; }
        public string Avatar_Url { get; set; }
        public IEnumerable<Book> Author_Books { get; set; }
    }
}
