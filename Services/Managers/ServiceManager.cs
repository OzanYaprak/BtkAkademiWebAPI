using AutoMapper;
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
        #region Constructor

        private readonly Lazy<IBookService> _bookService;

        public ServiceManager(IRepositoryManager repositoryManager, ILoggerService logger, IMapper mapper)
        {
            _bookService = new Lazy<IBookService>(() => new BookManager(repositoryManager, logger, mapper));
        }

        #endregion Constructor

        public IBookService BookService => _bookService.Value;
    }
}