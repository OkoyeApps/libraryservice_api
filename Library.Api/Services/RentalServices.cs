using Library.Domain.DbContext;
using Library.Domain.Entities;
using Library.Domain.Extensions;
using Library.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Library.Api.Services
{
    public class RentalServices : IRentalService
    {
        private readonly IMongoCollection<Borrowed_Book> _bookRentalCollection;
        private readonly IMongoCollection<Book> _bookCollection;
        private readonly IMongoClient MongoClient;
        private readonly ILogger<RentalServices> _logger;

        public RentalServices(IDbContext dbContext, ILogger<RentalServices> logger)
        {
            this._bookRentalCollection = dbContext?.Database.GetCollection<Borrowed_Book>($"{nameof(Borrowed_Book)}s").WithWriteConcern(WriteConcern.WMajority) ?? throw new ArgumentNullException(nameof(dbContext));
            this._bookCollection = dbContext?.Database.GetCollection<Book>($"{nameof(Book)}s").WithWriteConcern(WriteConcern.WMajority) ?? throw new ArgumentNullException(nameof(dbContext));
            this.MongoClient = dbContext.MongoClient;
            this._logger = logger;
        }

        private bool PerformRentOperation(ObjectId userId, ObjectId bookId, bool rent)
        {
            using (var session = MongoClient.StartSession())
            {
                var cancellationToken = CancellationToken.None;
                var bookToRent = new Borrowed_Book { Book = bookId, User = userId };
                var builder = Builders<Book>.Filter.Where(x => x.Id == bookId);
                int copies_available = 0;
                int copies_on_rent = 0;
                var updateTime = DateTime.UtcNow;
                if (rent)
                {
                    copies_available = -1;
                    copies_on_rent = 1;

                }
                else
                {
                    copies_available = 1;
                    copies_on_rent = -1;
                }
                var result = session.WithTransaction((s, ct) =>
                {
                    try
                    {
                        var ObjectToUpdate = new BsonDocument {
                            { "$inc", new BsonDocument{
                                {"Total_Copies_Available" , copies_available  },
                                { "Copies_on_Rent", copies_on_rent },
                                 { "UpdatedAt", updateTime.ToTimestamp() }
                            }  }
                        };
                        if (rent)
                        {
                            _bookRentalCollection.InsertOne(bookToRent, new InsertOneOptions { BypassDocumentValidation = false }, cancellationToken: cancellationToken);
                        }
                        else
                        {
                            var rentalBuilder = Builders<Borrowed_Book>.Filter.Where(x => x.Book == bookId && x.User == userId);
                            var ObjForUpdate = new BsonDocument
                            {
                                { "Returned", true },
                                { "UpdatedAt", updateTime.ToTimestamp() }
                            };
                        }
                        _bookCollection.UpdateOne(builder, ObjectToUpdate, cancellationToken: ct);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError("could not process rental service for user", ex);
                        return false;
                    }
                }, cancellationToken: cancellationToken);
                if (result) return true;
            }
            return false;
        }

        public async Task<bool?> RentBook(ObjectId userId, ObjectId BookId)
        {
            var bookQueryResult = await _bookCollection.FindAsync(x => x.Id == BookId);
            var bookDetail = bookQueryResult.FirstOrDefault();
            var BorrowedBook = await _bookRentalCollection.FindAsync(x => x.User == userId && x.Book == BookId);
            if (bookDetail is null) return false;
            else if (BorrowedBook.Any()) return null;

            return PerformRentOperation(userId, bookDetail.Id, true);
        }

        public async Task<bool?> ReturnRentedBook(ObjectId userId, ObjectId BookId)
        {
            var bookQueryResult = await _bookRentalCollection.FindAsync(x => x.Book == BookId && x.User == userId && x.Returned == false);
            var bookDetail = bookQueryResult.FirstOrDefault();
            if (bookDetail is null) return false;

            return PerformRentOperation(userId, bookDetail.Book, false);
        }
    }
}
