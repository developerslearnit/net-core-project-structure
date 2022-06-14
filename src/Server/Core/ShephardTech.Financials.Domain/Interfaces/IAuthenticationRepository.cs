using ShephardTech.Financials.Application;
using ShephardTech.Financials.Entities;

namespace ShephardTech.Financials.Domain.Interfaces
{
    public interface IAuthenticationRepository
    {
        IEnumerable<TodoViewModel> GetAll();
        Task<TodoViewModel> GetById(int id);
        Task<bool> AddTodo(TodoViewModel model);        
        
    }
}
