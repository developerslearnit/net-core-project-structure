using Microsoft.EntityFrameworkCore;
using ShephardTech.Financials.Application;
using ShephardTech.Financials.Domain.Interfaces;
using ShephardTech.Financials.Entities;
using ShephardTech.Financials.Persistence.StorageContexts.Financials;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ShephardTech.Financials.Domain.Repositories
{
    public class AuthenticationRepository : IAuthenticationRepository
    {
        private readonly ShepardFinContext _context;
        public AuthenticationRepository(ShepardFinContext context)
        {
            _context = context;
        }

        public async Task<bool> AddTodo(TodoViewModel model)
        {
            var todo = new Todo
            {
                Title = model.title,
                Content = model.content,
                Completed = model.completed,
                DueDate = model.dueDate,
                DateCreated = DateTime.Now
            };

            await _context.Todos.AddAsync(todo);

            return await _context.SaveChangesAsync() > 0;

        }

        public IEnumerable<TodoViewModel> GetAll()
        {
            //AsNoTracking improve query perfomance

            return _context.Todos.AsNoTracking().Select(t => new TodoViewModel
            {
                title = t.Title,
                content = t.Content,
                completed = t.Completed,
                dueDate = t.DueDate,
            });

        }
        
        public async Task<TodoViewModel> GetById(int id)
        {
            var todo = await _context.Todos.FindAsync(id);
            return new TodoViewModel
            {
                title = todo.Title,
                content = todo.Content,
                completed = todo.Completed,
                dueDate = todo.DueDate,
            };
        }
    }
}
