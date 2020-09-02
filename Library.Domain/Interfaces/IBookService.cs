using Library.Domain.Entities;
using Library.Domain.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Interfaces
{
    public interface IBookService
    {
        Task<(bool, string)> AddBook(Book AuthorModel);
        Task<Book> GetBookById(ObjectId Id);
        Task<PagedList<Book>> GetAllBooks(ResourceParameters resourceParams);
        Task<IEnumerable<Book>> GetBooksByAuthorId(ObjectId Author);
    }
}
