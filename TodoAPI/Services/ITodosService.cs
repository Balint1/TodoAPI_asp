using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAPI.Models;
using TodoAPI.Repositories;

namespace TodoAPI.Services
{
    public interface ITodosService
    {
        Task<List<Todo>> GetTodos(SortingType sortingType = SortingType.TimeDESC);
        Task<Todo> DeleteTodo(int id);
        Task<Todo> CreateTodo(Todo todo);
        Task<Todo> UpdateTodo(int id, Todo todo);
        Task<Todo> GetTodo(int id);
        Task<List<Todo>> GetTodos(bool done, SortingType sortingType = SortingType.TimeDESC);
        Task<List<Todo>> GetTodos(TodoCategory todoType, SortingType sortingType = SortingType.TimeDESC);
        Task<List<Todo>> GetArchivedTodos(SortingType sortingType = SortingType.TimeDESC);
        Task<List<Todo>> GetDeletedTodos(SortingType sortingType = SortingType.TimeDESC);
    }

}
