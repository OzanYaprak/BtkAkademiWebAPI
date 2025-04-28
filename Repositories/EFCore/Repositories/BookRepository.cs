using Entities.Models;
using Entities.RequestFeatures;
using Microsoft.EntityFrameworkCore;
using Repositories.EFCore.Base;
using Repositories.EFCore.Context;
using Repositories.EFCore.Extensions;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore.Repositories
{
    public sealed class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(RepositoryContext context) : base(context)
        {
        }

        public void CreateOneBook(Book book)
        {
            Create(book);
        }

        public void DeleteOneBook(Book book)
        {
            Delete(book);
        }

        public void UpdateOneBook(Book book)
        {
            Update(book);
        }

        // Veya methodlar aşağıdakiler gibi scope içerisine alınmadan döndürülebilinir.

        public async Task<Book> GetOneBookByIdAsync(int id, bool trackChanges) => await FindByCondition(x => x.Id == id, trackChanges)
            .FirstOrDefaultAsync();

        //public async Task<IEnumerable<Book>> GetAllBooksAsync(BookParameters bookParameters, bool trackChanges) => await FindAll(trackChanges)
        //    .OrderBy(x => x.Id)
        //    .Skip((bookParameters.PageNumber - 1) * bookParameters.PageSize)
        //    .Take(bookParameters.PageSize)
        //    .ToListAsync();

        public async Task<PagedList<Book>> GetAllBooksAsync(BookParameters bookParameters, bool trackChanges)
        {
            //var books = await FindAll(trackChanges).OrderBy(x => x.Id).ToListAsync();
            var books = await FindAll(trackChanges).FilterBooks(bookParameters.MinPrice, bookParameters.MaxPrice).Search(bookParameters.SearchTerm).OrderBy(x => x.Id).ToListAsync();

            return PagedList<Book>.ToPagedList(books, bookParameters.PageNumber, bookParameters.PageSize);
        }
    }
}