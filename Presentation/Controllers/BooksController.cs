using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;
using Entities.Models;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/books2")]
    public class BooksController : ControllerBase
    {
        #region Dependency Injection

        private readonly IServiceManager _manager;

        public BooksController(IServiceManager manager)
        {
            _manager = manager;
        }

        #endregion Dependency Injection

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            try
            {
                var books = _manager.BookService.GetAllBooks(false);
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
                var book = _manager.BookService.GetOneBookById(id, false);

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
                    return BadRequest(); //400
                }

                _manager.BookService.Create(book);

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
                if (book is null) { return BadRequest(); } //404

                _manager.BookService.Update(id, book, true);

                return NoContent(); // 204
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
                _manager.BookService.Delete(id, false);

                return NoContent();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        //[HttpDelete("{id:int}")]
        //public IActionResult PartiallyUpdateBook([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<Book> bookPatch)
        //{
        //    try
        //    {
        //        var entity = _manager.BookService.GetOneBookById(id, true);

        //        if (entity is null)
        //        {
        //            return NotFound(); // 404
        //        }

        //        bookPatch.ApplyTo(entity);
        //        _manager.BookService.Update(entity);

        //        return NoContent();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
    }
}