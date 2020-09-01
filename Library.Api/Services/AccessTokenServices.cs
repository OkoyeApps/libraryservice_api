using Library.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.Api.Services
{
    public class AccessTokenServices : IAccessTokenService
    {
        public Task<bool> IsActiveAsync(string token)
        {
            throw new NotImplementedException();
        }
    }
}
