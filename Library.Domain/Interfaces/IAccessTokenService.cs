using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Library.Domain.Interfaces
{
   public  interface IAccessTokenService
    {
        Task<bool> IsActiveAsync(string token);
    }
}
