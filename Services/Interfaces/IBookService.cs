using Entities.DataTransferObjects;
using Entities.Models;
using Entities.RequestFeatures;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IBookService
    {
        //Task<IEnumerable<BookDTO>> GetAllBooksAsync(BookParameters bookParameters, bool trackChanges);
        //Task<(IEnumerable<BookDTO> books, MetaData metaData)> GetAllBooksAsync(BookParameters bookParameters, bool trackChanges);
        Task<(IEnumerable<ExpandoObject> books, MetaData metaData)> GetAllBooksAsync(BookParameters bookParameters, bool trackChanges);

        Task<BookDTO> GetOneBookByIdAsync(int id, bool trackChanges);

        Task<BookDTO> CreateAsync(BookDTOForInsertion book);

        Task UpdateAsync(int id, BookDTOForUpdate bookDto, bool trackChanges);

        Task DeleteAsync(int id, bool trackChanges);

        Task SaveChangesForPatchAsync(BookDTOForUpdate bookDTOForUpdate, Book book);

        Task<(BookDTOForUpdate bookDTOForUpdate, Book book)> GetOneBookForPatchAsync(int id, bool trackChanges); // (BookDTOForUpdate bookDTOForUpdate, Book book) -> Bu kısım bir tuple ifadesi
    }
}