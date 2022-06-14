using ShephardTech.Financials.Domain.Interfaces;
using ShephardTech.Financials.Domain.Repositories;
using ShephardTech.Financials.Persistence.StorageContexts.Financials;

namespace ShephardTech.Financials.Domain
{
    public class RepositoryManager : IRepositoryManager
    {

        private ShepardFinContext _context;
        private IAuthenticationRepository _todoRepository;

        public RepositoryManager(ShepardFinContext context)
        {
            _context = context;
        }

        public IAuthenticationRepository AuthService
        {
            get
            {
                if (_todoRepository == null)
                {
                    _todoRepository = new AuthenticationRepository(_context);
                }
                return _todoRepository;
            }
        }
    }
}
