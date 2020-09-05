using Library.Domain.DbContext;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Domain.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Api.Services
{
    public class BookServices : IBookService
    {
        private readonly IMongoCollection<Book> Collection;
        private readonly ILogger<BookServices> _logger;

        public BookServices(IDbContext dbContext, ILogger<BookServices> logger)
        {
            this.Collection = dbContext?.Database.GetCollection<Book>($"{nameof(Book)}s") ?? throw new ArgumentNullException(nameof(dbContext));
            this._logger = logger;
        }
        public async Task<(bool, string)> AddBook(Book BookModel)
        {
            try
            {
                await Collection.InsertOneAsync(BookModel, new InsertOneOptions {  BypassDocumentValidation = false});
                return (true, "Author added succesfully");
            }
            catch (Exception ex)
            {
                _logger.LogError("saving of Author failed", ex);
                return (false, ex.Message);
            }
        }

        public async Task<PagedList<Book>> GetAllBooks(ResourceParameters resourceParams)
        {
            var queryalbe = Collection.AsQueryable();
            return PagedList<Book>.Create(queryalbe, resourceParams.PageNumber, resourceParams.PageSize);
        }

        public async Task<Book> GetBookById(ObjectId Id)
        { 
            var result =  await Collection.FindAsync(x => x.Id == Id);
            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<Book>> GetBooksByAuthorId(ObjectId Author)
        {
            var result =  await Collection.FindAsync(Builders<Book>.Filter.Eq("Author", Author));
            return result.ToList();
        }
    }
}
