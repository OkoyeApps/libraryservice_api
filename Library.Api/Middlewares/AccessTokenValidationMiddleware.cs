using Library.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Library.Api.Middlewares
{
    public class AccessTokenValidationMiddleware : IMiddleware
    {
        private readonly IAccessTokenService _accessTokenService;

        public AccessTokenValidationMiddleware(IAccessTokenService accessTokenService)
        {
            _accessTokenService = accessTokenService;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var number = 1;
            //fix this function later
            if (number == 1)
            {
                await next(context);
                return;
            }
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
        }
    }
}
