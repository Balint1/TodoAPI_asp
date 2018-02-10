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
            CreateMap<TodoType, TodoTypeView>();
        }
    }
}
