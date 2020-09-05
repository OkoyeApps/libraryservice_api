using System.Collections.Generic;
using Library.Domain.Interfaces;
using Library.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.Api.Controllers
{
    [ApiController]
    [Route("api/home")]
    public class HomeController : ControllerBase
    {
        public HomeController()
        {
          
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
    }
}
