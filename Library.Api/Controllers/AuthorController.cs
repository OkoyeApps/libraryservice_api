using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Domain.Models;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Library.Api.Controllers
{
    [Route("api/author")]
    [ApiController]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;
        private readonly IResourceUtil _resourceUtil;

        public AuthorController(IAuthorService authorService, IResourceUtil resourceUtil)
        {
            this._authorService = authorService;
            this._resourceUtil = resourceUtil;
        }

        /// <summary>
        /// Get all Authors
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet("index", Name = "GetAuthors")]
        public async Task<IActionResult> Index([FromQuery] ResourceParameters resourceParams)
        {
            var result = await _authorService.GetAllAuthors(resourceParams);
            var paginationMetadata = new
            {
                totalCount = result.TotalCount,
                pageSize = result.PageSize,
                currentPage = result.CurrentPage,
                totalPages = result.TotalPages,
            };
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
            var links = _resourceUtil.CreateLinksFoPaginations("GetAuthors", resourceParams, result.HasNext, result.HasPrevious);

            return Ok(new { value = result, links });
        }

        /// <summary>
        /// Get authors by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>

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

        /// <summary>
        /// Add Authors
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Get all books by a specific author
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
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
