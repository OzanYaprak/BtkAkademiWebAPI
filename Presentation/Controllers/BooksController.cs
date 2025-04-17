using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.Mvc;
using Services.Interfaces;

namespace Presentation.Controllers
{
    [ApiController]
    [Route("api/books")]
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
            var books = _manager.BookService.GetAllBooks(false);
            return Ok(books);
        }

        [HttpGet("{id:int}")]
        public IActionResult GetOneBook([FromRoute(Name = "id")] int id)
        {
            var book = _manager.BookService.GetOneBookById(id, false);

            return Ok(book);
        }

        [HttpPost]
        public IActionResult CreateBook([FromBody] Book book)
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

        [HttpPut("{id:int}")]
        public IActionResult UpdateBook([FromRoute(Name = "id")] int id, [FromBody] BookDTOForUpdate bookDto)
        {
            if (bookDto is null) { return BadRequest(); } //404

            _manager.BookService.Update(id, bookDto, true);

            return NoContent(); // 204
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteBook([FromRoute(Name = "id")] int id)
        {
            _manager.BookService.Delete(id, false);

            return NoContent();
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