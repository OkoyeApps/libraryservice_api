using Library.Domain.Extensions;
using Library.Domain.Interfaces;
using Library.Domain.Models;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Library.Api.Services
{
    public class JwtServices : IJwtHandler
    {
        private static readonly ISet<string> DefaultClaims = new HashSet<string>
        {
            JwtRegisteredClaimNames.Sub,
            JwtRegisteredClaimNames.UniqueName,
            JwtRegisteredClaimNames.Jti,
            JwtRegisteredClaimNames.Iat,
        };

        private readonly JwtSecurityTokenHandler _jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        private readonly JwtOptions _options;
        private readonly SigningCredentials _signingCredentials;
        private readonly TokenValidationParameters _tokenValidationParameters;

        public JwtServices(JwtOptions options)
        {
            _options = options;
            var issurerSigninKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.SecretKey));
            _signingCredentials = new SigningCredentials(issurerSigninKey, SecurityAlgorithms.HmacSha256);
            _tokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = issurerSigninKey,
                ValidIssuer = _options.Issuer,
                ValidAudience = _options.ValidAudience,
                ValidateAudience = false,
            };
        }


        public JsonWebToken GenerateToken(string userId, IDictionary<string, string> claims = null)
        {
            if (string.IsNullOrWhiteSpace(userId))
            {
                throw new ArgumentException("User id for claim can not be empty.", nameof(userId));
            }
            var currentTime = DateTime.UtcNow;

            var jwtClaims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, userId),
                new Claim(JwtRegisteredClaimNames.UniqueName, userId),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Iat, currentTime.ToTimestamp().ToString()),
            };

            var customClaims = claims?.Select(claim => new Claim(claim.Key, claim.Value)).ToArray() ?? Array.Empty<Claim>();
            jwtClaims.AddRange(customClaims);
            var expires = currentTime.AddDays(_options.ExpiryMinutes);
            var jwt = new JwtSecurityToken(
                issuer: _options.Issuer,
                claims: jwtClaims,
                expires: expires,
                notBefore: currentTime,
                signingCredentials: _signingCredentials
                );

            var token = new JwtSecurityTokenHandler().WriteToken(jwt);
            return new JsonWebToken
            {
                AccessToken = token,
                Expires = expires.ToTimestamp(),
                Id = userId,
                Claims = customClaims
            };
        }


        public JsonWebTokenPayload GetTokenPayload(string accessToken)
        {
            _jwtSecurityTokenHandler.ValidateToken(accessToken, _tokenValidationParameters, out var validatedSecurityToken);
            if(!(validatedSecurityToken is JwtSecurityToken jwt))
            {
                return null;
            }
            return new JsonWebTokenPayload
            {
                Subject = jwt.Subject,
                Expires = jwt.ValidTo.ToTimestamp(),
                Claims = jwt.Claims.Where(x => DefaultClaims.Contains(x.Type)).ToArray()
            };
        }
    }
}

