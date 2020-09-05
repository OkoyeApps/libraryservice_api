
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Library.Domain.Dtos;
using Library.Domain.Entities;
using Library.Domain.Interfaces;
using Library.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace Library.Api.Controllers
{
    [Route("api/book")]
    [ApiController]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;
        private readonly IRentalService _rentalService;
        private readonly IResourceUtil _resourceUtil;
        private readonly IMapper _mapper;

        public BookController()
        {

        }
        public BookController(IBookService bookService, IRentalService rentalService, IResourceUtil resourceUtil, IMapper mapper)
        {
            this._bookService = bookService;
            this._rentalService = rentalService;
            this._resourceUtil = resourceUtil;
            this._mapper = mapper;
        }
        /// <summary>
        /// Get all books available
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet("index", Name ="GetAllBooks")]
        public async Task<IActionResult> Index([FromQuery] ResourceParameters resourceParams)
        {
            var result = await _bookService.GetAllBooks(resourceParams);
            var paginationMetadata = new
            {
                totalCount = result.TotalCount,
                pageSize = result.PageSize,
                currentPage = result.CurrentPage,
                totalPages = result.TotalPages,
            };
            Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));
            var links = _resourceUtil.CreateLinksFoPaginations("GetAllBooks", resourceParams, result.HasNext, result.HasPrevious);
            
            return Ok(new {value = result, links });
        }

        /// <summary>
        /// Get Books by Id
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
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

        /// <summary>
        /// Add Book
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>

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

        /// <summary>
        /// Rent Book
        /// </summary>
        /// <param name="Book"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("rent", Name = "RentBooK")]
        public async Task<IActionResult> RentBook(BookRentDto Book)
        {
            var parsed = ObjectId.TryParse(Book.BookId, out ObjectId parsedId);
            if (!parsed)
            {
                return BadRequest();
            }

            var result = await _rentalService.RentBook(ObjectId.Parse((string)Request.Headers["user_id"]), parsedId);
            if (result is null) return BadRequest(new { message = "You can't borrow this book as you haven't returned the previous copy you borrowed" });

            if (!result.Value)
            {
                return BadRequest(new { message = "could not process request"});
            }
            return Ok(new { message = "request completed" });
        }

        /// <summary>
        /// Return rented book
        /// </summary>
        /// <param name="Book"></param>
        /// <returns></returns>

        [Authorize]
        [HttpPost("returnbook", Name = "ReturnRentedBooK")]
        public async Task<IActionResult> ReturnBook(BookRentDto Book)
        {
            var parsed = ObjectId.TryParse(Book.BookId, out ObjectId parsedId);
            if (!parsed)
            {
                return BadRequest();
            }

            var result = await _rentalService.ReturnRentedBook(ObjectId.Parse((string)Request.Headers["user_id"]), parsedId);
             if(result is null) return BadRequest(new { message = "Book already returned" });

            else if (!result.Value) return BadRequest(new { message = "could not process request" }); 
            return Ok(new { message = "request completed" });
        }

    }
}
