using Library.Domain.DbContext;
using Library.Domain.DBQueries;
using Library.Domain.Dtos;
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
    public class AuthorServices : IAuthorService
    {
        private readonly IMongoCollection<Author> Collection;
        private readonly AuthorQueries _authorQueries;
        private readonly ILogger<AuthorServices> _logger;

        public AuthorServices(IDbContext dbContext, AuthorQueries authorQueries, ILogger<AuthorServices> logger)
        {
            this.Collection = dbContext?.Database.GetCollection<Author>($"{nameof(Author)}s") ?? throw new ArgumentNullException(nameof(dbContext));
            this._authorQueries = authorQueries;
            this._logger = logger;
        }
        public async Task<(bool, string)> AddAuthor(Author AuthorModel)
        {
            try
            {
                await Collection.InsertOneAsync(AuthorModel, new InsertOneOptions {  BypassDocumentValidation = false});
                return (true, "Author added succesfully");
            }
            catch (Exception ex)
            {
                _logger.LogError("Adding of author failed", ex);
                return (false, ex.Message);
            }
        }

        public async Task<PagedList<Author>> GetAllAuthors(ResourceParameters resourceParams)
        {
            var queryalbe = Collection.AsQueryable();
            return await Task.FromResult(PagedList<Author>.Create(queryalbe, resourceParams.PageNumber, resourceParams.PageSize));
        }

        public async Task<Author> GetAuthorById(ObjectId Id)
        {
            var result =  await Collection.FindAsync(x => x.Id == Id);
            return result.FirstOrDefault();
            
        }

        public async Task<IEnumerable<AuthorAndBooksDto>> GetAuthorsAndAuthorBooks(ObjectId AuthorId)
        {
            var pipeLine = _authorQueries.GenerateQueryForAutherAndBooks(AuthorId);

            var result =  Collection.Aggregate<AuthorAndBooksDto>(pipeLine);

            return await result.ToListAsync();
        }
    }
}
