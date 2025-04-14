using Repositories.Interfaces;
using Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.Managers
{
    public class ServiceManager : IServiceManager
    {
        private readonly Lazy<IBookService> _bookService;

        public ServiceManager(IRepositoryManager repositoryManager)
        {
            _bookService = new Lazy<IBookService>(() => new BookManager(repositoryManager));
        }

        public IBookService BookService => _bookService.Value;
    }
}