using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Interfaces;
using System.Text.Json;

namespace Presentation.Controllers
{
    [ServiceFilter(typeof(LogFilterAttribute))] // Controller Seviyesinde Sistem
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
        public async Task<IActionResult> GetAllBooksAsync([FromQuery] BookParameters bookParameters)
        {
            //var books = await _manager.BookService.GetAllBooksAsync(bookParameters, false);
            var pagedResult = await _manager.BookService.GetAllBooksAsync(bookParameters, false);

            Response.Headers.Append("X-Pagination", JsonSerializer.Serialize(pagedResult.metaData));
            //Test
            return Ok(pagedResult.books);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetOneBookAsync([FromRoute(Name = "id")] int id)
        {
            var book = await _manager.BookService.GetOneBookByIdAsync(id, false);

            return Ok(book);
        }

        // Method Seviyesinde Sistem [ServiceFilter(typeof(LogFilterAttribute), Order = 2)] // Order -> İşlem sırasını belirler
        [ServiceFilter(typeof(ValidationFilterAttribute)/*, Order = 1*/)]
        [HttpPost]
        public async Task<IActionResult> CreateBookAsync([FromBody] BookDTOForInsertion bookDto)
        {
            // ValidationFilterAttribute Kısmına Taşındı
            //if (bookDto is null)
            //{
            //    return BadRequest(); //400
            //}

            //if (!ModelState.IsValid)
            //{
            //    return UnprocessableEntity(ModelState); // 422
            //}

            var book = await _manager.BookService.CreateAsync(bookDto);

            //return StatusCode(201, book); // Mesaj ile döndürmek istiyorsak aşağıdaki yöntem
            return StatusCode(201, new
            {
                message = "Başarılı",
                data = book
            });
        }

        [ServiceFilter(typeof(ValidationFilterAttribute))]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> UpdateBookAsync([FromRoute(Name = "id")] int id, [FromBody] BookDTOForUpdate bookDto)
        {
            // ValidationFilterAttribute Kısmına Taşındı
            //if (bookDto is null) { return BadRequest(); } //400

            //if (!ModelState.IsValid)
            //{
            //    return UnprocessableEntity(ModelState); // 422
            //}

            await _manager.BookService.UpdateAsync(id, bookDto, false);

            return NoContent(); // 204
        }

        [HttpDelete("{id:int}")]
        public async Task<IActionResult> DeleteBookAsync([FromRoute(Name = "id")] int id)
        {
            await _manager.BookService.DeleteAsync(id, false);

            return NoContent();
        }

        [HttpPatch("{id:int}")]
        public async Task<IActionResult> PartiallyUpdateBookAsync([FromRoute(Name = "id")] int id, [FromBody] JsonPatchDocument<BookDTOForUpdate> bookPatch)
        {
            if (bookPatch is null) { return BadRequest(); } // 400

            var result = await _manager.BookService.GetOneBookForPatchAsync(id, false);

            bookPatch.ApplyTo(result.bookDTOForUpdate, ModelState);

            TryValidateModel(result.bookDTOForUpdate);

            if (!ModelState.IsValid) { return UnprocessableEntity(ModelState); } // 422

            await _manager.BookService.SaveChangesForPatchAsync(result.bookDTOForUpdate, result.book);

            return NoContent(); // 204
        }
    }
}