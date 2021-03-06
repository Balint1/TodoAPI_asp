using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Mime;
using System.Text;
using TodoAPI;
using TodoAPI.ViewModels;
using Xunit;

namespace IntegrationTests
{
    public class TodoIntegrationTests
    {
        private readonly TestServer _server;
        private readonly HttpClient _client;
        public TodoIntegrationTests()
        {
            // Arrange
            _server = new TestServer(new WebHostBuilder()
                .UseStartup<Startup>());
            _client = _server.CreateClient();
            
        }

        [Fact]
        public async void GetTodoByIdTest()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/todos/2");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
            var res =
            "{\"id\":2,\"title\":\"Sajt\",\"isDone\":false,\"type\":{\"id\":2,\"name\":\"Bevásárlás\",\"argb\":120},\"creationDate\":\"2018-02-11T12:10:24.55\"}";
            //TodoView result = JObject.Parse(responseString).ToObject<TodoView>();
            // Assert
            Assert.Equal(res,responseString);
        }
        [Fact]
        public async void DoneTodosTest()
        {
            // Act
            var response = await _client.GetAsync("/api/v1/todos/true");
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();
           
            var array = JArray.Parse(responseString);
            

            // Assert
            Assert.Equal(3,array.Count);
        }
        [Fact]
        public async void PostTodoTest()
        {
            string uriString = "/api/v1/todos/";
            // Act
            var response = await _client.GetAsync(uriString);
            response.EnsureSuccessStatusCode();

            var responseString = await response.Content.ReadAsStringAsync();

            var beforeArray = JArray.Parse(responseString);
            var category = new TodoCategoryView { Id=3,Name = "TESTCategory", Argb = 120 };
            var todo = new TodoView { Title = "Test", IsDone = false, Type = category };
            JObject todoJSON = (JObject)JToken.FromObject(todo);
            //HttpContent content = new StringContent("{\"title\":\"Alma\",\"isDone\":false,\"type\":{\"id\":15,\"name\":\"fsadf\",\"argb\":0},\"creationDate\":\"2018-02-10T11:58:25.2233333\"}",Encoding.UTF8, "application/json");
            HttpContent content = new StringContent(todoJSON.ToString(),Encoding.UTF8, "application/json");
            response = await _client.PostAsync(uriString, content);
            response.EnsureSuccessStatusCode();

            response = await _client.GetAsync(uriString);
            response.EnsureSuccessStatusCode();

            responseString = await response.Content.ReadAsStringAsync();

            var afterArray = JArray.Parse(responseString);

            // Assert
            Assert.Equal(beforeArray.Count+1, afterArray.Count);
        }
    }
}
