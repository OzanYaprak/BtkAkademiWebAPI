using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Exceptions;
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
        private readonly IMapper _mapper;

        public BookManager(IRepositoryManager manager, ILoggerService logger, IMapper mapper)
        {
            _manager = manager;
            _logger = logger;
            _mapper = mapper;
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
                //string message = $"The book with id:{id} could not found.";
                //_logger.LogInfo(message);
                //throw new Exception(message);

                throw new BookNotFoundException(id);
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
            var book = _manager.BookRepository.GetOneBookById(id, trackChanges);

            if (book == null) { throw new BookNotFoundException(id); }

            return book;
        }

        public void Update(int id, BookDTOForUpdate bookDto, bool trackChanges)
        {
            // Check Entity
            var entity = _manager.BookRepository.GetOneBookById(id, trackChanges);
            if (entity is null)
            {
                //string message = $"Book with id:{id} could not found.";
                //_logger.LogInfo(message);
                //throw new Exception(message);

                throw new BookNotFoundException(id);
            }

            entity = _mapper.Map<Book>(bookDto);

            _manager.BookRepository.Update(entity);
            _manager.Save();
        }
    }
}