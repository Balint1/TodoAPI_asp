using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.Configuration;
using Microsoft.AspNetCore.Authorization;
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
    //[Route("/api/[controller]")]
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
       // [Route("/api/v1/[controller]/GetAll")]
       // [Route("/api/v1/[controller]")]
        public async Task<IActionResult> GetTodos([FromQuery] string category,[FromQuery] bool archived)
        {
            var user = User;
            _logger.LogInformation($"GET todos category : {category} archived : {archived}");
            List<Todo> todos;
            if(category != null)
            {
                    todos = await _todosService.GetTodos(category,SortingType.TimeDESC);
            }
            else
                if(archived)
                {
                    todos= await _todosService.GetArchivedTodos();
                }
                else
                    todos = await _todosService.GetTodos();

            //return Ok(_mapper.Map<List<TodoView>>(todos));
            var todoViews = new List<TodoView>();
            foreach (var item in todos)
            {
                todoViews.Add(_mapper.Map<TodoView>(item));
            }
            return Ok(todoViews);
        }
        // GET: api/Todos/true
        [HttpGet("{done:bool}")]
        public async Task<IActionResult> GetTodos(bool done)
        {
            _logger.LogInformation($"GET Done todos ");
            var todos = await _todosService.GetTodos(done);
            //return Ok(_mapper.Map<IEnumerable<TodoView>>(todos));
            var todoViews = new List<TodoView>();
            foreach (var item in todos)
            {
                todoViews.Add(_mapper.Map<TodoView>(item));
            }
            return Ok(todoViews);
        }

        // GET: api/Todos/5
        //[Authorize]
        //[ValidateAntiForgeryToken]
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
        //[Authorize]
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
        [Authorize]
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