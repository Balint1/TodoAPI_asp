using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using TodoAPI.Services;
using TodoAPI.ViewModels;

namespace TodoAPI.Controllers.v1
{
    [Produces("application/json")]
    [Route("/api/[controller]")]
    public class CategoryController : Controller
    {

        
        private readonly IMapper _mapper;
        private readonly ITodosService _todosService;
        private readonly ILogger<TodosController> _logger;

        public CategoryController(IMapper mapper, ITodosService todosService, ILogger<TodosController> logger)
        {
            _mapper = mapper;
            _todosService = todosService;
            _logger = logger;
        }

        public bool CategoryView { get; private set; }

        // GET: api/Category
        [HttpGet]
        public async Task<IActionResult> GetCategories()
        {
            _logger.LogInformation($"GET Todo categories");
            var categories = await _todosService.GetCategories();
            var categoriViews = _mapper.Map<List<TodoCategoryView>>(categories);
            return Ok(categoriViews);
        }
    }
}