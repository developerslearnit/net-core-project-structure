using ShephardTech.Financials.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShephardTech.Financials.Domain
{
    public interface IRepositoryManager
    {
        IAuthenticationRepository AuthService { get; }
    }
}
