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

        Book GetOneBookById(int id, bool trackChanges);

        Book Create(Book book);

        void Update(int id, BookDTOForUpdate bookDto, bool trackChanges);

        void Delete(int id, bool trackChanges);
    }
}