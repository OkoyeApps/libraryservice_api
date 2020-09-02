using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Interfaces
{
    public interface IRentalService
    {
        Task<bool?> RentBook(ObjectId userId, ObjectId BookId);
        Task<bool?> ReturnRentedBook(ObjectId userId, ObjectId BookId);
    }
}
