using Entities.Models;
using Repositories.EFCore.Base;
using Repositories.EFCore.Context;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.EFCore.Repositories
{
    public class BookRepository : RepositoryBase<Book>, IBookRepository
    {
        public BookRepository(RepositoryContext context) : base(context)
        {
        }

        public void CreateOneBook(Book book) => Create(book);
        public void DeleteOneBook(Book book) => Delete(book);
        public void UpdateOneBook(Book book) => Update(book);


        public IQueryable<Book> GetAllBooks(bool trackChanges) => FindAll(trackChanges).OrderBy(x => x.Id);
        public IQueryable<Book> GetOneBookById(int id, bool trackChanges) => FindByCondition(x => x.Id == id, trackChanges);

    }
}
