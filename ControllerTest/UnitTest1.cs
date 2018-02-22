using AutoMapper;
using AutoMapper.Configuration;
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
        Mock<ILogger<TodosService>> mockServiceLogger;
        Mock<IMapper> mockMapper;
        TodosController controller;
        List<Todo> testTodos = new List<Todo>();
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
            var todos = Assert.IsAssignableFrom<List<TodoView>>(
               OkObject.Value);
            Assert.Equal(3, todos.Count());
            
        }
        [Fact]
        public async Task GetArchivedTodosTest()
        {
            // Arrange
            init();
            mockRepo.Setup(repo => repo.GetArchivedTodos(SortingType.TimeDESC)).Returns(Task.FromResult(GetTestArchivedTodos()));

            // Act
            IActionResult result = await controller.GetTodos(null,true);


            // Assert
            var OkObject = Assert.IsType<OkObjectResult>(result);
            var todos = Assert.IsAssignableFrom<IEnumerable<TodoView>>(
               OkObject.Value);
            Assert.Equal(3, todos.Count());

        }

        private void init()
        {
            mockLogger = new Mock<ILogger<TodosController>>();
            mockServiceLogger = new Mock<ILogger<TodosService>>();
            service = new TodosService(mockRepo.Object,mockServiceLogger.Object);
            mockMapper = new Mock<IMapper>();
            //var mappings = new MapperConfigurationExpression();
            //mappings.AddProfile<DomainProfile>();
            //Mapper.Initialize(mappings);
            Mapper.Initialize(cfg => { 
                cfg.CreateMap<Todo, TodoView>();
                cfg.CreateMap<TodoCategory, TodoCategoryView>();
                cfg.CreateMap<TodoView, Todo>();
                cfg.CreateMap<TodoCategoryView, TodoCategory>();
            });

            controller = new TodosController(mockMapper.Object, service, mockLogger.Object);

            TodoCategory category = new TodoCategory(1, "Bevásárlás", 127); 
            TodoCategory category1 = new TodoCategory(2, "Teendõk", 156565);

            testTodos.Add(new Todo(1, "Korte", category));
            testTodos.Add(new Todo(2, "Szilva", category));
            testTodos.Add(new Todo(3, "Kitakarítani", category1));
            testTodos.ElementAt(0).Archived = true;

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
            todos.Add(new Todo(2, "Korte", category));
            todos.Add(new Todo(3, "Szilva", category));
            foreach (var todo in todos)
            {
                todo.Archived = true;
            }
            return todos;
        }
    }
}
