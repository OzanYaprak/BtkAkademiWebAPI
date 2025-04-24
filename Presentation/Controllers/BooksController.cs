using Entities.DataTransferObjects;
using Entities.Models;
using Microsoft.AspNetCore.JsonPatch;
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
        public IActionResult CreateBook([FromBody] BookDTOForInsertion bookDto)
        {
            if (bookDto is null)
            {
                return BadRequest(); //400
            }

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState); // 422
            }

            var book = _manager.BookService.Create(bookDto);

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
            if (bookDto is null) { return BadRequest(); } //400

            if (!ModelState.IsValid)
            {
                return UnprocessableEntity(ModelState); // 422
            }

            _manager.BookService.Update(id, bookDto, false);

            return NoContent(); // 204
        }

        [HttpDelete("{id:int}")]
        public IActionResult DeleteBook([FromRoute(Name = "id")] int id)
        {
            _manager.BookService.Delete(id, false);

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public IActionResult PartiallyUpdateBook([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<BookDTOForUpdate> bookPatch)
        {
            if (bookPatch is null) { return BadRequest(); } // 400

            var result = _manager.BookService.GetOneBookForPatch(id, false);

            bookPatch.ApplyTo(result.bookDTOForUpdate, ModelState);

            TryValidateModel(result.bookDTOForUpdate);

            if (!ModelState.IsValid) { return UnprocessableEntity(ModelState); } // 422

            _manager.BookService.SaveChangesForPatch(result.bookDTOForUpdate, result.book);

            return NoContent(); // 204
        }
    }
}