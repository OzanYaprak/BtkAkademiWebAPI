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

        public BookDTO Create(BookDTOForInsertion bookDto)
        {
            var entity = _mapper.Map<Book>(bookDto);

            _manager.BookRepository.Create(entity);
            _manager.Save();

            return _mapper.Map<BookDTO>(entity); ;
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

        public IEnumerable<BookDTO> GetAllBooks(bool trackChanges)
        {
            var books = _manager.BookRepository.GetAllBooks(trackChanges);
            var mapper = _mapper.Map<IEnumerable<BookDTO>>(books);
            return mapper;
        }

        public BookDTO GetOneBookById(int id, bool trackChanges)
        {
            var book = _manager.BookRepository.GetOneBookById(id, trackChanges);

            if (book == null) { throw new BookNotFoundException(id); }

            return _mapper.Map<BookDTO>(book);
        }

        public (BookDTOForUpdate bookDTOForUpdate, Book book) GetOneBookForPatch(int id, bool trackChanges)
        {
            var book = _manager.BookRepository.GetOneBookById(id, trackChanges);

            if (book == null) { throw new BookNotFoundException(id); }

            var bookDtoForUpdate = _mapper.Map<BookDTOForUpdate>(book);

            return (bookDtoForUpdate, book); // Tuple olarak dönüyoruz.
        }

        public void SaveChangesForPatch(BookDTOForUpdate bookDTOForUpdate, Book book)
        {
            _mapper.Map(bookDTOForUpdate, book);
            _manager.Save();
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