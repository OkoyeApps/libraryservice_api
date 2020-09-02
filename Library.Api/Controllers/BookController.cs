
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Library.Domain.Dtos;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Library.Api.Controllers
{
    [Route("api/book/")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IMapper _mapper;

        public BookController(IBookService bookService, IMapper mapper)
        {
            this._bookService = bookService;
            this._mapper = mapper;
        }
        public async Task<IActionResult> Index()
        {

            return Ok(await _bookService.GetAllBooks());
        }

        [HttpGet("{Id}", Name = "GetBookById")]
        public async Task<IActionResult> GetAuthorById(string Id)
        {
            var parsed = ObjectId.TryParse(Id, out ObjectId parsedId);
            if (!parsed)
            {
                return BadRequest();
            }

            var result = await _bookService.GetBookById(parsedId);
            if (result is null) return NotFound();
            return Ok(result);
        }

        [HttpPost(Name = "AddBook")]
        public async Task<IActionResult> AddAuthor(BookDto model)
        {
            var bookModel = _mapper.Map<BookDto, Book>(model);
            var result = await _bookService.AddBook(bookModel);
            if (!result.Item1)
            {
                return BadRequest(new { message = result.Item2 });
            }

            return CreatedAtRoute("GetBookById", new { Id = bookModel.Id.ToString() }, model);
        }

    }
}
