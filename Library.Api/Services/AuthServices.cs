
using Library.Domain.DbContext;
using Library.Domain.Dtos;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Library.Api.Services
{
    public class AuthServices : IAuthService
    {
        private readonly IMongoCollection<User> Collection;
        private readonly ILogger<AuthServices> _logger;
        public AuthServices(IDbContext dbContext, ILogger<AuthServices> logger)
        {
            this.Collection = dbContext?.Database.GetCollection<User>($"{nameof(User)}s") ?? throw new ArgumentNullException(nameof(dbContext));
            this._logger = logger;
        }
        public string GenerateHash(string password)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            // derive a 256-bit subkey (use HMACSHA1 with 10,000 iterations)
            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed;
        }

        public async Task<bool> RegisterUser(User userModel)
        {
            try
            {
                await Collection.InsertOneAsync(userModel, new InsertOneOptions { BypassDocumentValidation = false });
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("User creation failed", ex);
                return false;
            }
        }
    }
}
