using Azure;
using Entities.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Repositories.EFCore.Context;
using Repositories.Interfaces;
using WebApi.Repositories;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        #region Dependency Injection

        private readonly IRepositoryManager _manager;

        public BooksController(IRepositoryManager manager)
        {
            _manager = manager;
        }

        #endregion Dependency Injection

        [HttpGet]
        public IActionResult GetAllBooks()
        {
            try
            {
                var books = _manager.BookRepository.GetAllBooks(false);
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
                var book = _manager.BookRepository.GetOneBookById(id, false);

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

                _manager.BookRepository.Create(book);
                _manager.Save();

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
                var entity = _manager.BookRepository.GetOneBookById(id, true);

                if (entity is null) { return NotFound(); } // 404
                if (id != book.Id) { return BadRequest(); } // 400

                entity.Title = book.Title;
                entity.Price = book.Price;

                _manager.Save();

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
                var entity = _manager.BookRepository.GetOneBookById(id, false);

                if (entity is null)
                {
                    return NotFound(new { statusCode = 404, message = $"Book with id:{id} could not found." }); // 404
                }

                _manager.BookRepository.Delete(entity);
                _manager.Save();

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
        //        var entity = _manager.BookRepository.GetOneBookById(id, true);

        //        if (entity is null)
        //        {
        //            return NotFound(); // 404
        //        }

        //        bookPatch.ApplyTo(entity);
        //        _manager.BookRepository.Update(entity);

        //        return NoContent();
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
    }
}