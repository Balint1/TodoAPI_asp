using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAPI.Controllers;
using TodoAPI.Models;
using TodoAPI.Repositories;
using TodoAPI.Services;
using TodoAPI.ViewModels;
using Xunit;

namespace ControllerTest
{
    public class UnitTest1
    {
        Mock<ITodosRepository> mockRepo = new Mock<ITodosRepository>();
        TodosService service;
        Mock<ILogger<TodosController>> mockLogger;
        Mock<IMapper> mockMapper;
        TodosController controller;
       [Fact]
        public async Task GetTodosTest()
        {
            // Arrange
            init();
            mockRepo.Setup(repo => repo.GetTodos(SortingType.TimeDESC)).Returns(Task.FromResult(GetTestTodos()));

            // Act
            IActionResult result = await controller.GetTodos(null,false);


            // Assert
            var OkObject = Assert.IsType<OkObjectResult>(result);
            var todos = Assert.IsAssignableFrom<IEnumerable<TodoView>>(
               OkObject.Value);
            Assert.Equal(3, todos.Count());
            
        }
        [Fact]
        public async Task GetArchivedTodosTest()
        {
            // Arrange
            init();
            mockRepo.Setup(repo => repo.GetTodos(true,SortingType.TimeDESC)).Returns(Task.FromResult(GetTestArchivedTodos()));

            // Act
            IActionResult result = await controller.GetTodos(true);


            // Assert
            var OkObject = Assert.IsType<OkObjectResult>(result);
            var todos = Assert.IsAssignableFrom<IEnumerable<TodoView>>(
               OkObject.Value);
            Assert.Equal(3, todos.Count());

        }

        private void init()
        {
            service = new TodosService(mockRepo.Object);
            mockLogger = new Mock<ILogger<TodosController>>();
            mockMapper = new Mock<IMapper>();
            controller = new TodosController(mockMapper.Object, service, mockLogger.Object);


        }
        private List<Todo> GetTestTodos()
        {
            TodoCategory category = new TodoCategory(1,"Bevásárlás",127);
            List<Todo> todos = new List<Todo>();
            todos.Add(new Todo(1, "Alma", category));
            todos.Add(new Todo(1, "Korte", category));
            todos.Add(new Todo(1, "Szilva", category));
            return todos;
        }
        private List<Todo> GetTestArchivedTodos()
        {
            TodoCategory category = new TodoCategory(1, "Bevásárlás", 127);
            List<Todo> todos = new List<Todo>();
            todos.Add(new Todo(1, "Alma", category));
            todos.Add(new Todo(1, "Korte", category));
            todos.Add(new Todo(1, "Szilva", category));
            foreach (var todo in todos)
            {
                todo.Archived = true;
            }
            return todos;
        }
    }
}
