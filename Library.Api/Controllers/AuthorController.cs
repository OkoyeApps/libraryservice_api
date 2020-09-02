using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Library.Api.Controllers
{
    [Route("api/author/")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            this._authorService = authorService;
        }
        public IActionResult Index()
        {
            return Ok();
        }

        [HttpGet("{Id}", Name ="GetAuthorById")]
        public async Task<IActionResult> GetAuthorById(string Id)
        {
          var parsed =   ObjectId.TryParse(Id, out ObjectId parsedId);
            if (!parsed)
            {
                return BadRequest();
            }

            var result = await _authorService.GetAuthorById(parsedId);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpPost(Name ="AddAuthor")]
        public async Task<IActionResult> AddAuthor(Author model)
        {
            var result = await _authorService.AddAuthor(model);
            if (!result.Item1)
            {
                return BadRequest(new { message = result.Item2 });
            }

            return CreatedAtRoute("GetAuthorById", new { Id = model.Id.ToString() }, model);
        }

        [HttpGet("{Id}/allbooks", Name ="GetAuthorAndBooks")]
        public async Task<IActionResult> GetAuthorAndBooks(string Id)
        {
            var parsed = ObjectId.TryParse(Id, out ObjectId parsedId);
            if (!parsed)
            {
                return BadRequest();
            }

            var result = await _authorService.GetAuthorsAndAuthorBooks(parsedId);
            if (result is null) return NotFound();
            return Ok(result);
        }

    }
}
