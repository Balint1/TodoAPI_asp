using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAPI.Models;
using TodoAPI.ViewModels;

namespace TodoAPI.Controllers
{
    public class DomainProfile : Profile
    {
        public DomainProfile()
        {
            CreateMap<Todo, TodoView>();
            CreateMap<TodoCategory, TodoCategoryView>();
            CreateMap<TodoView, Todo>();
            CreateMap<TodoCategoryView, TodoCategory>();
            //CreateMap<List<Todo>, List<TodoView> > ();
            //CreateMap<List<TodoCategory>, List<TodoCategoryView>>();
            //CreateMap<List<TodoView>, List<Todo>>();
            //CreateMap< List<TodoCategoryView>, List<TodoCategory>>();
            //CreateMap<IEnumerable<Todo>, IEnumerable<TodoView>>();
            //CreateMap<IEnumerable<TodoCategory>, IEnumerable<TodoCategoryView>>();
            //CreateMap<IEnumerable<TodoView>, IEnumerable<Todo>>();
            //CreateMap<IEnumerable<TodoCategoryView>, IEnumerable<TodoCategory>>();
        }
    }
}
 