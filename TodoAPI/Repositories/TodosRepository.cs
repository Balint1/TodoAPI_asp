using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAPI.Models;

namespace TodoAPI.Repositories
{
    public class TodosRepository : ITodosRepository
    {
        private readonly TodoContext _context;
        private readonly ILogger<TodosRepository> _logger;

        public TodosRepository(TodoContext context, ILogger<TodosRepository> logger)
        {
            _context = context;
            _logger = logger;
        }
        public async Task<Todo> CreateTodo(Todo todo)
        {
            var type = await _context.Types.FirstOrDefaultAsync(t => t.Id.Equals(todo.Type.Id));
            if (type != null) todo.Type = type;
            _context.Todos.Add(todo);
            await _context.SaveChangesAsync();
            return todo;
        }

        public async Task<Todo> DeleteTodo(int id)
        {
            _logger.LogInformation($"Deleting Todo : {id}");
            var todo = await _context.Todos.SingleOrDefaultAsync(m => m.Id == id );
            if (todo == null) {
                _logger.LogWarning($"Deleting Todo : {id} Not Found!");
                return todo;
            }
            todo.Deleted = true;
            await _context.SaveChangesAsync();
            _logger.LogInformation($"Todo : {id} deleted succesfully");
            return todo;
        }

        public async Task<List<Todo>> GetArchivedTodos(SortingType sortingType = SortingType.TimeDESC)
        {
            _logger.LogInformation($"Get archived todos sorting by  : {sortingType} ");
            var todos = await _context.Todos
                .Where(t => t.Archived == true && t.Deleted == false)
                .OrderByDescending(t => t.CreationDate)
                .ToListAsync();
            if (todos == null)
            {
                _logger.LogWarning("  Zero todo found!");
                return null;
            }
            return todos;
        }

        public async Task<List<Todo>> GetDeletedTodos(SortingType sortingType = SortingType.TimeDESC)
        {
            _logger.LogInformation($"Get deleted todos sorting by  : {sortingType} ");
            var todos = await _context.Todos
                .Where(t => t.Deleted == true)
                .OrderByDescending(t => t.CreationDate)
                .ToListAsync();
            if (todos == null)
            {
                _logger.LogWarning("  Zero todo found!");
                return null;
            }
            return todos;
        }

        public async Task<Todo> GetTodo(int id)
        {
            _logger.LogInformation($"Get Todo Id : {id}");
            var todo = await _context.Todos.FirstOrDefaultAsync(t => t.Id == id && !t.Deleted );
            if (todo == null)
            {
                _logger.LogWarning($" Todo Id: {id} Not Found!");
                return todo;
            }
            return todo;
        }

        public async Task<List<Todo>> GetTodos(SortingType sortingType = SortingType.TimeDESC)
        {
            _logger.LogInformation($"Get Todos sorting by  : {sortingType}");
            var todos = await _context.Todos
                .Include(todo => todo.Type)
                .Where(t => t.Deleted == false && t.Archived == false)
                .OrderByDescending(t => t.CreationDate)
                .ToListAsync();
            if (todos == null)
            {
                _logger.LogWarning("  Zero todo found!");
                return null;
            }
            return todos;
        }

        public async Task<List<Todo>> GetTodos(bool done, SortingType sortingType = SortingType.TimeDESC)
        {
            _logger.LogInformation($"Get Todos sorting by  : {sortingType} Done : {done}");
            var todos = await _context.Todos.Where(t => t.IsDone == done && t.Deleted == false && t.Archived == false)
                .OrderByDescending(t => t.CreationDate)
                .ToListAsync();
            if (todos == null)
            {
                _logger.LogWarning("Zero todo found!");
                return null;
            }
            return todos;
        }

        public async Task<List<Todo>> GetTodos(TodoCategory todoType, SortingType sortingType = SortingType.TimeDESC)
        {
            _logger.LogInformation($"Get Todos sorting by  : {sortingType} TodoType : {todoType}");
            var todos = await _context.Todos
                .Where(t => t.Type.Equals(todoType))
                .OrderByDescending(t => t.CreationDate)
                .ToListAsync();
            if (todos == null)
            {
                _logger.LogWarning("Zero todo found!");
                return null;
            }
            return todos;
        }

        public async Task<Todo> UpdateTodo(int id, Todo todo)
        {
            _context.Entry(todo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return todo;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoExists(id))
                {
                    return null;
                }
                else
                {
                    throw ;
                }
            }
        }

        public TodoCategory FindCategoryByName(string categoryName)
        {
            return _context.Types.FirstOrDefault(t => t.Name.Equals(categoryName));
        }
        private bool TodoExists(int id)
        {
            return _context.Todos.Any(e => e.Id == id);
        }
    }
}
