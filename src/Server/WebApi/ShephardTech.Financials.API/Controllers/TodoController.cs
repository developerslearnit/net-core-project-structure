using Microsoft.AspNetCore.Mvc;
using ShephardTech.Financials.Application;
using ShephardTech.Financials.Application.Contracts;
using ShephardTech.Financials.Domain;

namespace ShephardTech.Financials.API.Controllers
{
    [ApiVersion("1.0")]
    [Route("api/v{v:apiversion}")]

    public class TodoController : BaseController
    {
        private readonly IRepositoryManager _service;

        public TodoController(IRepositoryManager service)
        {
            _service = service;
        }


        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ApiResponse<List<TodoViewModel>>), StatusCodes.Status200OK)]
        [HttpGet("todos")]
        public IActionResult GetTodos()
        {
            var todos = _service.AuthService.GetAll();
            return StatusCode(StatusCodes.Status200OK,
                   new ApiResponse<List<TodoViewModel>>
                   {
                       Data = todos.ToList(),
                       hasError = false,
                       StatusCode = StatusCodes.Status200OK
                   });
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ApiResponse<TodoViewModel>), StatusCodes.Status200OK)]
        [HttpGet("todos/{id}")]
        public async Task<IActionResult> GetTodoById([FromRoute] int id)
        {
            var todo = await _service.AuthService.GetById(id);
            return StatusCode(StatusCodes.Status200OK,
                   new ApiResponse<TodoViewModel>
                   {
                       Data = todo,
                       hasError = false,
                       StatusCode = StatusCodes.Status200OK
                   });
        }

        [ProducesResponseType(200)]
        [ProducesResponseType(typeof(ApiResponse<List<TodoViewModel>>), StatusCodes.Status200OK)]
        [HttpPost("todos")]
        public async Task<IActionResult> AddTodo([FromBody] TodoViewModel model)
        {

            try
            {

                model.dueDate = DateTime.Now;
                
                await _service.AuthService.AddTodo(model);
                return StatusCode(StatusCodes.Status200OK,
                       new ApiResponse<object>
                       {
                           Data = null,
                           hasError = false,
                           StatusCode = StatusCodes.Status200OK
                       });
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError,
                       new ApiResponse<object>
                       {
                           Data = null,
                           hasError = true,
                           StatusCode = StatusCodes.Status500InternalServerError
                       });
            }
          
            
        }

    }
}
