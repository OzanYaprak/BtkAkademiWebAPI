using Entities.Models;
using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Managers
{
    public class BookManager : IBookService
    {
        #region Constructor

        private readonly IRepositoryManager _manager;
        private readonly ILoggerService _logger;

        public BookManager(IRepositoryManager manager, ILoggerService logger)
        {
            _manager = manager;
            _logger = logger;
        }

        #endregion Constructor

        public Book Create(Book book)
        {
            _manager.BookRepository.Create(book);
            _manager.Save();
            return book;
        }

        public void Delete(int id, bool trackChanges)
        {
            // Check Entity
            var entity = _manager.BookRepository.GetOneBookById(id, trackChanges);
            if (entity is null)
            {
                string message = $"The book with id:{id} could not found.";
                _logger.LogInfo(message);
                throw new Exception(message);
            }

            _manager.BookRepository.Delete(entity);
            _manager.Save();
        }

        public IEnumerable<Book> GetAllBooks(bool trackChanges)
        {
            return _manager.BookRepository.GetAllBooks(trackChanges);
        }

        public Book GetOneBookById(int id, bool trackChanges)
        {
            return _manager.BookRepository.GetOneBookById(id, trackChanges);
        }

        public void Update(int id, Book book, bool trackChanges)
        {
            // Check Entity
            var entity = _manager.BookRepository.GetOneBookById(id, trackChanges);
            if (entity is null)
            {
                string message = $"Book with id:{id} could not found.";
                _logger.LogInfo(message);
                throw new Exception(message);
            }

            entity.Title = book.Title;
            entity.Price = book.Price;

            _manager.BookRepository.Update(entity);
            _manager.Save();
        }
    }
}