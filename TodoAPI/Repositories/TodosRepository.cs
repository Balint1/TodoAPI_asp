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
            _logger.LogDebug($"todo : {todo.Id} created succesfully");
            return todo;
        }

        public async Task<Todo> DeleteTodo(int id)
        {
            _logger.LogDebug($"Deleting Todo : {id}");
            var todo = await _context.Todos
                .IgnoreQueryFilters()
                .Include(t => t.Type)
                .SingleOrDefaultAsync(m => m.Id == id );
            if (todo == null) {
                _logger.LogWarning($"Deleting Todo : {id} Not Found!");
                return todo;
            }
            todo.Deleted = true;
            await _context.SaveChangesAsync();
            _logger.LogDebug($"Todo : {id} deleted succesfully");
            return todo;
        }

        public async Task<List<Todo>> GetArchivedTodos(SortingType sortingType = SortingType.TimeDESC)
        {
            var todos = await _context.Todos
                .Include(todo => todo.Type)
                .IgnoreQueryFilters()
                .Where(t => t.Archived && !t.Deleted)
                .OrderByDescending(t => t.CreationDate)
                .ToListAsync();
            if (todos == null)
            {
                _logger.LogWarning("  Zero todo found!");
                return null;
            }
            _logger.LogDebug($"Got archived todos sorting by  : {sortingType} ");
            return todos;
        }

        public async Task<List<Todo>> GetDeletedTodos(SortingType sortingType = SortingType.TimeDESC)
        {
            var todos = await _context.Todos
                .IgnoreQueryFilters()
                .Where(t => t.Deleted == true)
                .OrderByDescending(t => t.CreationDate)
                .ToListAsync();
            if (todos == null)
            {
                _logger.LogWarning("Zero todo found!");
                return null;
            }
            _logger.LogInformation($"Got deleted todos sorting by  : {sortingType} ");
            return todos;
        }

        public async Task<Todo> GetTodo(int id)
        {
            var todo = await _context.Todos
                .IgnoreQueryFilters()
                .Where(t => !t.Deleted)
                .Include(t => t.Type)
                .FirstOrDefaultAsync(t => t.Id == id);
            if (todo == null)
            {
                _logger.LogWarning($" Todo Id: {id} Not Found!");
                return todo;
            }
            _logger.LogDebug($"Got Todo Id : {id}");
            return todo;
        }
        
        public async Task<List<Todo>> GetTodos(SortingType sortingType = SortingType.TimeDESC)
        {
            var todos = await _context.Todos
                .Include(todo => todo.Type)
                .OrderByDescending(t => t.CreationDate)
                .ToListAsync();
            if (todos == null)
            {
                _logger.LogWarning("  Zero todo found!");
                return null;
            }
            _logger.LogDebug($"Got Todos sorting by  : {sortingType}");
            return todos;
        }

        public async Task<List<Todo>> GetTodos(bool done, SortingType sortingType = SortingType.TimeDESC)
        {
            var todos = await _context.Todos
                .Where(t => t.IsDone == done)
                .Include(todo => todo.Type)
                .OrderByDescending(t => t.CreationDate)
                .ToListAsync();
            if (todos == null)
            {
                _logger.LogWarning("Zero todo found!");
                return null;
            }
            _logger.LogDebug($"Got Todos sorting by  : {sortingType} Done : {done}");
            return todos;
        }

        public async Task<List<Todo>> GetTodos(TodoCategory todoType, SortingType sortingType = SortingType.TimeDESC)
        {
            var todos = await _context.Todos
                .Include(todo => todo.Type)
                .Where(t => t.Type.Equals(todoType))
                .OrderByDescending(t => t.CreationDate)
                .ToListAsync();
            if (todos == null)
            {
                _logger.LogWarning("Zero todo found!");
                return null;
            }
            _logger.LogDebug($"Got Todos sorting by  : {sortingType} TodoType : {todoType}");
            return todos;
        }

        public async Task<Todo> UpdateTodo(int id, Todo todo)
        {
            _context.Entry(todo).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                _logger.LogDebug($"Todo : {id} updated succesfully");
                return todo;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TodoExists(id))
                {
                    _logger.LogWarning($"Todo : {id} Not Found");
                    return null;
                }
                else
                {
                    _logger.LogError($"Todo : {id} DbUpdateConcurrencyException");
                    throw ;
                }
            }
        }

        public TodoCategory FindCategoryByName(string categoryName)
        {
            _logger.LogDebug($"CategoryName : {categoryName} got succesfully");
            return _context.Types.FirstOrDefault(t => t.Name.Equals(categoryName));
        }

        public async Task<List<TodoCategory>> GetTodoCategories()
        {
            var categories = await _context.Types
                .OrderByDescending(t => t.Name)
                .ToListAsync();
            if (categories == null)
            {
                _logger.LogWarning("  Zero category found!");
                return null;
            }
            _logger.LogDebug($"Got Categories");
            return categories;
        }
        private bool TodoExists(int id)
        {
            return _context.Todos.Any(e => e.Id == id);
        }

    }
}
