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
            var token =(string) context.Request.Headers["Authorization"];
            if (!string.IsNullOrWhiteSpace(token))
            {
                token = token.Replace("Bearer ", "");
                var result = _accessTokenService.IsActiveAsync(token);
                if (result.Item1)
                {
                    context.Request.Headers["user_id"] = result.Item2.Subject;
                    await next(context);
                    return;
                }
                context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
                return;
            }
            await next(context);
        }
    }
}
