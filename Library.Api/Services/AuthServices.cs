
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
using BCrypt.Net;
using Library.Domain.Models;

namespace Library.Api.Services
{
    public class AuthServices : IAuthService
    {
        private readonly IMongoCollection<User> Collection;
        private readonly ILogger<AuthServices> _logger;
        private readonly IJwtHandler _jwtHandler;

        public AuthServices(IDbContext dbContext, ILogger<AuthServices> logger, IJwtHandler jwtHandler)
        {
            this.Collection = dbContext?.Database.GetCollection<User>($"{nameof(User)}s") ?? throw new ArgumentNullException(nameof(dbContext));
            this._logger = logger;
            this._jwtHandler = jwtHandler;
        }

        public async Task<bool> RegisterUser(User userModel)
        {
            try
            {
                userModel.Password = BCrypt.Net.BCrypt.HashPassword(userModel.Password);
                await Collection.InsertOneAsync(userModel, new InsertOneOptions { BypassDocumentValidation = false });
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError("User creation failed", ex);
                return false;
            }
        }

        public async Task<JsonWebToken> SigninUser(UserAuthDto model)
        {
            var queryResult = await Collection.FindAsync(x => x.Username == model.Username);
            var userDetails = queryResult.FirstOrDefault();
            if (userDetails is null)
            {
                return null;
            }
            var isValidPassword = BCrypt.Net.BCrypt.Verify(model.Password, userDetails.Password);
            if (!isValidPassword)
            {
                return null;
            }
            return _jwtHandler.GenerateToken(userDetails.Id.ToString(), null);
        }
    }
}
