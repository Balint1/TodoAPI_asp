﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAPI.Exceptions;
using TodoAPI.Models;
using TodoAPI.Repositories;

namespace TodoAPI.Services
{
    public class TodosService : ITodosService
    {
        private readonly ITodosRepository _todoRepository;
        public TodosService(ITodosRepository todoRepository)
        {
            _todoRepository = todoRepository;
        }

        async Task<Todo> ITodosService.CreateTodo(Todo todo)
        {
            return await _todoRepository.CreateTodo(todo);
        }

        async Task<Todo> ITodosService.DeleteTodo(int id)
        {
            return await _todoRepository.DeleteTodo(id);
        }

        async Task<List<Todo>> ITodosService.GetArchivedTodos(SortingType sortingType)
        {
            return await _todoRepository.GetArchivedTodos(sortingType);
        }

        async Task<List<Todo>> ITodosService.GetDeletedTodos(SortingType sortingType)
        {
            return await _todoRepository.GetDeletedTodos(sortingType);
        }

        async Task<Todo> ITodosService.GetTodo(int id)
        {
            return await _todoRepository.GetTodo(id);
        }

        async Task<List<Todo>> ITodosService.GetTodos(SortingType sortingType)
        {
            return await _todoRepository.GetTodos(sortingType);
        }

        async Task<List<Todo>> ITodosService.GetTodos(bool done, SortingType sortingType)
        {
            return await _todoRepository.GetTodos(done, sortingType);
        }

        async Task<List<Todo>> ITodosService.GetTodos(string todoType, SortingType sortingType)
        {
            TodoCategory todoCategory = _todoRepository.FindCategoryByName(todoType);
            if (todoCategory == null) throw new CategoryNotFoundException("Nem található ilyen kategória");
            return await _todoRepository.GetTodos(todoCategory, sortingType);
        }

        async Task<Todo> ITodosService.UpdateTodo(int id, Todo todo)
        {
            return await _todoRepository.UpdateTodo(id, todo);
        }
    }
}
