using Entities.DataTransferObjects;
using Entities.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Interfaces
{
    public interface IBookService
    {
        IEnumerable<BookDTO> GetAllBooks(bool trackChanges);

        BookDTO GetOneBookById(int id, bool trackChanges);

        BookDTO Create(BookDTOForInsertion book);

        void Update(int id, BookDTOForUpdate bookDto, bool trackChanges);

        void Delete(int id, bool trackChanges);
    }
}