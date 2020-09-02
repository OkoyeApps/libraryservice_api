using Library.Domain.Dtos;
using Library.Domain.Entities;
using Library.Domain.Models;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Interfaces
{
    public interface IAuthorService 
    {
        Task<(bool, string)> AddAuthor(Author AuthorModel);
        Task<Author> GetAuthorById(ObjectId Id);
        Task<IEnumerable<AuthorAndBooksDto>> GetAuthorsAndAuthorBooks (ObjectId AuthorId);
        Task<PagedList<Author>> GetAllAuthors(ResourceParameters resourceParams);
    }
}
