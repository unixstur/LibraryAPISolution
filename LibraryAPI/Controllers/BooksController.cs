using AutoMapper;
using LibraryAPI.Data;
using LibraryAPI.Models.Books;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.QueryableExtensions;
using Microsoft.AspNetCore.Http;

namespace LibraryAPI.Controllers
{
    public class BooksController : ControllerBase
    {
        private readonly LibraryDataContext _context;
        private readonly MapperConfiguration _config;
        private readonly IMapper _mapper;

        public BooksController(LibraryDataContext context, MapperConfiguration config, IMapper mapper)
        {
            _context = context;
            _config = config;
            _mapper = mapper;
        }

        [HttpPut("books/{id:int}/genre")]
        public async Task<ActionResult> UpdateGenre(int id, [FromBody] string newGenre)
        {
            var book = await _context.GetBooksThatAreInInventory()
                .SingleOrDefaultAsync(b => b.Id == id);
            if (book != null)
            {
                book.Genre = newGenre;
                await _context.SaveChangesAsync();
            }
            return NoContent();
        }

        [HttpDelete("books/{id:int}")]
        public async Task<ActionResult> RemoveBookFromInventory(int id)
        {
            var savedBook = await _context.GetBooksThatAreInInventory()
                .SingleOrDefaultAsync(b => b.Id == id);

            if (savedBook != null)
            {
                savedBook.IsInInventory = false;
                await _context.SaveChangesAsync();
            }
            return NoContent();
        } 

        [HttpPost("books")]
        public async Task<ActionResult<GetBookDetailsResponse>> AddBook([FromBody] PostBookRequest bookToAdd)
        {
            // 1. Validate (if not, return a 400)
            // 2. Make the change
            //    a. Add it to the database (so map from PostBookCreate -> Book)
            //    b. Save the changes.
            // 3. Return a 201 Created
            //    a. With a location header of the new book
            //    b. A representation of the book they would get if they followed the location header
            //       1. so, map from Book -> GetBookDetailsResponse (We have this already!)
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            else
            {
                var book = _mapper.Map<Book>(bookToAdd);
                _context.Books.Add(book);

                await _context.SaveChangesAsync();
                var response = _mapper.Map<GetBookDetailsResponse>(book);
                return CreatedAtRoute("books#getbookdetails", new { id = response.Id }, response);
            }
        }

        /// <summary>
        /// Look up a book by Id
        /// </summary>
        /// <param name="id">Book Id</param>
        /// <returns>The requested book or a 404</returns>
        [HttpGet("books/{id:int}", Name = "books#getbookdetails")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<ActionResult<GetBookDetailsResponse>> GetBookDetails(int id)
        {
            var response = await _context.GetBooksThatAreInInventory()
                .Where(b => b.Id == id)
                .ProjectTo<GetBookDetailsResponse>(_config)
                .SingleOrDefaultAsync();

            if (response == null)
            {
                return NotFound();
            }
            else
            {
                return Ok(response);
            }
        }

        [HttpGet("books")]
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task <ActionResult<GetBooksResponseItem>> GetAllBooks()
        {
            var books = await _context.GetBooksThatAreInInventory()
                .ProjectTo<GetBooksResponseItem>(_config)
                .ToListAsync();

            var response = new GetBooksResponse
            {
                Data = books
            };

            return Ok(response);
        }
    }
}
