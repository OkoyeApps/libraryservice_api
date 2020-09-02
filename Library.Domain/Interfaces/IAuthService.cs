using Library.Domain.Dtos;
using Library.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterUser(User userModel);
        string GenerateHash(string password);
    }
}
