using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using TodoAPI.Exceptions;
using TodoAPI.Models;
using TodoAPI.Repositories;
using TodoAPI.Services;
using TodoAPI.ViewModels;

namespace TodoAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Todos")]
    public class TodosController : Controller
    {
        private readonly IMapper _mapper;
        private readonly ITodosService _todosService;
        private readonly ILogger<TodosController> _logger;

        public TodosController(IMapper mapper, ITodosService todosService, ILogger<TodosController> logger)
        {
            _mapper = mapper;
            _todosService = todosService;
            _logger = logger;
        }

       

        // GET: api/Todos
        [HttpGet]
        public async Task<IActionResult> GetTodos([FromQuery] string category,[FromQuery] bool archived)
        {
            _logger.LogInformation($"GET todos category : {category} archived : {archived}");
            List<Todo> todos;
            if(category != null)
            {
                    todos = await _todosService.GetTodos(category,SortingType.TimeDESC);
                //try
                //{
                //}
                //catch (CategoryNotFoundException e)
                //{
                //    return BadRequest(e.Message);
                //}
            }
            else
                if(archived)
                {
                    todos= await _todosService.GetArchivedTodos();
                }
                else
                    todos = await _todosService.GetTodos();
            var res = _mapper.Map<List<TodoView>>(todos);
            return Ok(res);
        }
        // GET: api/Todos/true
        [HttpGet("{done:bool}")]
        public async Task<IActionResult> GetTodos(bool done)
        {
            _logger.LogInformation($"GET Done todos ");
            var todos = await _todosService.GetTodos(done);
            return Ok(_mapper.Map<List<TodoView>>(todos));
        }

        // GET: api/Todos/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTodo([FromRoute] int id)
        {
            _logger.LogInformation($"GET todo Id : {id}");
            

            var todo = await _todosService.GetTodo(id);// _context.Todos.SingleOrDefaultAsync(m => m.Id == id);

            if (todo == null)
            {
                return NotFound("Nem található ilyen todo :(");
            }
            var todoView = _mapper.Map<TodoView>(todo);
            return Ok(todoView);
        }

        // PUT: api/Todos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodo([FromRoute] int id, [FromBody] TodoView todoView)
        {
            _logger.LogInformation($"UPDATE todo Id : {id}");

            if (id != todoView.Id)
            {
                return BadRequest();
            }
            Todo todo = _mapper.Map<Todo>(todoView);
            await _todosService.UpdateTodo(id,todo);

            return NoContent();
        }

        // POST: api/Todos
        [HttpPost]
        public async Task<IActionResult> PostTodo([FromBody] TodoView todoView)
        {
            _logger.LogInformation($"POST todo");
            if (todoView == null) return BadRequest();
            todoView.Id = 0;
            var todo = _mapper.Map<Todo>(todoView);
            todo = await _todosService.CreateTodo(todo);
            todoView = _mapper.Map<TodoView>(todo);
            return CreatedAtAction("GetTodo", new { id = todoView.Id }, todoView);
        }

        // DELETE: api/Todos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo([FromRoute] int id)
        {
            _logger.LogInformation($"DELETE todo Id : {id}");
            

            var todo = await _todosService.DeleteTodo(id);
            if (todo == null)
            {
                return NotFound();
            }
            var todoView = _mapper.Map<TodoView>(todo);
            return Ok(todoView);
        }

      
    }
}