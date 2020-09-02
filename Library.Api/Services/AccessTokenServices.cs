using Library.Domain.Interfaces;
using Library.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Api.Services
{
    public class AccessTokenServices : IAccessTokenService
    {
        private readonly IJwtHandler _jwtHandler;

        public AccessTokenServices(IJwtHandler jwtHandler)
        {
            _jwtHandler = jwtHandler;
        }
        public (bool, JsonWebTokenPayload) IsActiveAsync(string token)
        {
            var result = _jwtHandler.GetTokenPayload(token);
            if (result is null)
            {
                return (false, null);
            }
            return (true, result);
        }
    }
}
