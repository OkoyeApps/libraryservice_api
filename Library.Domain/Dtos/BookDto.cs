using System;
using System.Collections.Generic;
using System.Text;

namespace Library.Domain.Dtos
{
    public class BookDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string TotalPages { get; set; }
        public string Author { get; set; } //this is the author id as a reference here
        public string Isbn { get; set; }
        public long Total_Copies { get; set; }
        public string Publisher { get; set; }
    }
    public class BookRentDto
    {
        public string BookId { get; set; }
    }
}
