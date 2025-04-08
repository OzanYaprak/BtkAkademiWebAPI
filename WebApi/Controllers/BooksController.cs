using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories.EFCore;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        #region Dependency Injection

        private readonly RepositoryContext _context;

        public BooksController(RepositoryContext context)
        {
            _context = context;
        }

        #endregion

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            try
            {
                var books = _context.Books.ToList();
                return Ok(books);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
        {
            try
            {
                var book = _context.Books.Where(x => x.Id == id).FirstOrDefault();

                if (book == null) { return NotFound(); } //404

                return Ok(book);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpPost]
        public IActionResult CreateBook([FromBody] Book book)
        {
            try
            {
                if (book is null)
                {
                    return BadRequest(); //404
                }

                _context.Books.Add(book);
                _context.SaveChanges();

                //return StatusCode(201, book); // Mesaj ile döndürmek istiyorsak aşağıdaki yöntem
                return StatusCode(201, new
                {
                    message = "Başarılı",
                    data = book
                });
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpPut("{id:int}")]
        public IActionResult UpdateBook([FromRoute(Name = "id")] int id, [FromBody] Book book)
        {
            try
            {
                // check entity?
                var entity = _context.Books.Where(x => x.Id == id).FirstOrDefault();

                if (entity is null) { return NotFound(); } // 404
                if (id != book.Id) { return BadRequest(); } // 400


                entity.Title = book.Title;
                entity.Price = book.Price;

                _context.SaveChanges();

                return Ok(book);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteBook([FromRoute(Name = "id")] int id)
        {
            try
            {
                var entity = _context.Books.Where(x => x.Id.Equals(id)).FirstOrDefault();

                if (entity is null)
                {
                    return NotFound(new { statusCode = 404, message = $"Book with id:{id} could not found." }); // 404
                }

                _context.Books.Remove(entity);
                _context.SaveChanges();

                return NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
