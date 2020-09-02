﻿using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Library.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace Library.Api.Controllers
{
    [ApiController]
    [Route("api/home")]
    public class HomeController : ControllerBase
    {
        private readonly IJwtHandler _jwtHandler;

        public HomeController(IJwtHandler jwtHandler)
        {
            _jwtHandler = jwtHandler;
        }
        [Route("index")]
        public IActionResult Index()
        {
            return Ok("in the home");
        }

        
        [Route("Authenticate")]
        public IActionResult Authenticate()
        {
            var result = _jwtHandler.GenerateToken("1", new Dictionary<string, string> { { "granny", "oduko" } });
            return Ok(result);
        }

        [Route("secret")]
        [Authorize]
        public IActionResult Secret()
        {
            var aa = Request;
            return Ok();
        }
    }
}
