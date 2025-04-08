using Repositories.EFCore.Context;
using Repositories.EFCore.Repositories;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Managers
{
    public class RepositoryManager : IRepositoryManager
    {
        #region Constructor

        private readonly RepositoryContext _context;
        private readonly Lazy<IBookRepository> _bookRepository;
        public RepositoryManager(RepositoryContext context)
        {
            _context = context;
            _bookRepository = new Lazy<IBookRepository>(() => new BookRepository(_context));
        }

        #endregion

        public IBookRepository BookRepository => _bookRepository.Value;

        public void Save() => _context.SaveChanges();
    }
}
