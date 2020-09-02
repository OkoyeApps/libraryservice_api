using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Library.Domain.Interfaces;
using Library.Domain.Models;
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

        [HttpGet(Name = "GetRoot")]
        public IActionResult GetRoot()
        {
            var links = new List<LinkDto>();
            links.Add(new LinkDto(Url.Link("GetRoot", new { }), "self", "GET"));
            links.Add(new LinkDto(Url.Link("GetAuthors", new { }), "authors", "GET"));
            links.Add(new LinkDto(Url.Link("GetAllBooks", new { }), "books", "GET"));
            return Ok(links);

        }

        [HttpGet]
        [Route("secret")]
        [Authorize]
        public IActionResult Secret()
        {
            var aa = Request;
            return Ok();
        }
    }
}
