using AutoMapper;
using Entities.DataTransferObjects;
using Entities.Exceptions;
using Entities.Models;
using Entities.RequestFeatures;
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

        public async Task<BookDTO> CreateAsync(BookDTOForInsertion bookDto)
        {
            var entity = _mapper.Map<Book>(bookDto);

            _manager.BookRepository.Create(entity);
            await _manager.SaveAsync();

            return _mapper.Map<BookDTO>(entity); ;
        }

        public async Task DeleteAsync(int id, bool trackChanges)
        {
            var entity = await GetOneBookByIdAndCheckExists(id, trackChanges);
            _manager.BookRepository.Delete(entity);
            await _manager.SaveAsync();
        }

        public async Task<(IEnumerable<BookDTO> books, MetaData metaData)> GetAllBooksAsync(BookParameters bookParameters, bool trackChanges)
        {
            //var books = await _manager.BookRepository.GetAllBooksAsync(bookParameters, trackChanges);
            var booksWithMetaData = await _manager.BookRepository.GetAllBooksAsync(bookParameters, trackChanges);
            //var mapper = _mapper.Map<IEnumerable<BookDTO>>(books);
            var booksDTO = _mapper.Map<IEnumerable<BookDTO>>(booksWithMetaData);
            return (booksDTO, booksWithMetaData.MetaData);
        }

        public async Task<BookDTO> GetOneBookByIdAsync(int id, bool trackChanges)
        {
            var book = await GetOneBookByIdAndCheckExists(id, trackChanges);

            return _mapper.Map<BookDTO>(book);
        }

        public async Task<(BookDTOForUpdate bookDTOForUpdate, Book book)> GetOneBookForPatchAsync(int id, bool trackChanges)
        {
            var book = await GetOneBookByIdAndCheckExists(id, trackChanges);

            var bookDtoForUpdate = _mapper.Map<BookDTOForUpdate>(book);

            return (bookDtoForUpdate, book); // Tuple olarak dönüyoruz.
        }

        public async Task UpdateAsync(int id, BookDTOForUpdate bookDto, bool trackChanges)
        {
            // Check Entity
            var entity = await GetOneBookByIdAndCheckExists(id, trackChanges);

            entity = _mapper.Map<Book>(bookDto);

            _manager.BookRepository.Update(entity);
            await _manager.SaveAsync();
        }

        #region Helper Methods

        private async Task<Book> GetOneBookByIdAndCheckExists(int id, bool trackChanges)
        {
            // Check Entity
            var entity = await _manager.BookRepository.GetOneBookByIdAsync(id, trackChanges);
            if (entity is null)
            {
                //string message = $"The book with id:{id} could not found.";
                //_logger.LogInfo(message);
                //throw new Exception(message);

                throw new BookNotFoundException(id);
            }

            return entity;
        }

        public async Task SaveChangesForPatchAsync(BookDTOForUpdate bookDTOForUpdate, Book book)
        {
            _mapper.Map(bookDTOForUpdate, book);
            await _manager.SaveAsync();
        }

        #endregion Helper Methods
    }
}