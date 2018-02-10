using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TodoAPI.Models;
using TodoAPI.Repositories;
using TodoAPI.ViewModels;

namespace TodoAPI.Controllers
{
    [Produces("application/json")]
    [Route("api/Todos")]
    public class TodosController : Controller
    {
        private readonly TodoContext _context;
        private readonly IMapper _mapper;
        private readonly ITodosRepository _todosRepository;

        public TodosController(TodoContext context, IMapper mapper,ITodosRepository todosRepository)
        {
            _context = context;
            _mapper = mapper;
            _todosRepository = todosRepository;
        }

        // GET: api/Todos
        [HttpGet]
        public async Task<IActionResult> GetTodos()
        {
            //return _mapper.Map<TodoView>(_context.Todos.Include(todo => todo.Type));

            var todos = await _todosRepository.GetTodos();//_context.Todos.Include(todo => todo.Type);
            var todoViews = new List<TodoView>();
            foreach (var ttodo in todos)
            {
                todoViews.Add(_mapper.Map<TodoView>(ttodo));
            }
            return Ok(todoViews);
        }

        // GET: api/Todos/5
        [HttpGet("{id}")]
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
        public async Task<IActionResult> PostTodo([FromBody] Todo todo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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
    }
}