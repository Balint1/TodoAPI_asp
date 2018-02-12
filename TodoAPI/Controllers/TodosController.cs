﻿using System;
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
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

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
            todoView.Id = 0;
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var todo = _mapper.Map<Todo>(todoView);
            todo = await _todosService.CreateTodo(todo);
            todoView = _mapper.Map<TodoView>(todo);
            return CreatedAtAction("GetTodo", new { id = todoView.Id }, todoView);
        }

        // DELETE: api/Todos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTodo([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var todo = await _todosService.DeleteTodo(id);
            if (todo == null)
            {
                return NotFound();
            }
            var todoView = _mapper.Map<TodoView>(todo);
            return Ok(todoView);
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