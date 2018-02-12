using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        private readonly TodoContext _context;
        private readonly IMapper _mapper;
        private readonly ITodosService _todosService;
        private readonly ITodosRepository _todosrepo;

        public TodosController(TodoContext context, IMapper mapper,ITodosService todosService, ITodosRepository todosRepository)
        {
            _context = context;
            _mapper = mapper;
            _todosService = todosService;
            _todosrepo = todosRepository;
        }

        // GET: api/Todos
        [HttpGet]
        public async Task<IActionResult> GetTodos([FromQuery] string category,[FromQuery] bool archived)
        {
            List<Todo> todos;

            if(category != null)
            {
                try
                {
                    todos = await _todosService.GetTodos(category);
                }
                catch (CategoryNotFoundException e)
                {
                    return BadRequest(e.Message);
                }
            }
            else
                if(archived)
                {
                    todos= await _todosService.GetArchivedTodos();
                }
                else
                    todos = await _todosService.GetTodos();
            return Ok(mapTodoToTodoView(todos));
        }
        // GET: api/Todos/true
        [HttpGet("{done:bool}")]
        public async Task<IActionResult> GetTodos(bool done)
        {

            var todos = await _todosService.GetTodos(done);
            return Ok(mapTodoToTodoView(todos));
        }

        // GET: api/Todos/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetTodo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var todo = await _context.Todos.SingleOrDefaultAsync(m => m.Id == id);

            if (todo == null)
            {
                return NotFound();
            }

            return Ok(todo);
        }

        // PUT: api/Todos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutTodo([FromRoute] int id, [FromBody] Todo todo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != todo.Id)
            {
                return BadRequest();
            }

            _context.Entry(todo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Todos
        [HttpPost]
        public async Task<IActionResult> PostTodo([FromBody] TodoView todoView)
        {
            todoView.Id = 0;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var todo = _mapper.Map<Todo>(todoView);
            var type = await _context.Types.FirstOrDefaultAsync(t => t.Id.Equals(todoView.Type.Id) && t.Name.Equals(todoView.Type.Name));
            if (type != null) todo.Type = type;
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTodo", new { id = todo.Id }, todo);
        }

        // DELETE: api/Todos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var todo = await _context.Todos.SingleOrDefaultAsync(m => m.Id == id);
            if (todo == null)
            {
                return NotFound();
            }

            _context.Todos.Remove(todo);
            await _context.SaveChangesAsync();

            return Ok(todo);
        }

        private bool TodoExists(int id)
        {
            return _context.Todos.Any(e => e.Id == id);
        }
        private List<TodoView> mapTodoToTodoView(IEnumerable<Todo> todos)
        {
            var todoViews = new List<TodoView>();
            foreach (var todo in todos)
            {
                todoViews.Add(_mapper.Map<TodoView>(todo));
            }
            return todoViews;
        }
    }
}