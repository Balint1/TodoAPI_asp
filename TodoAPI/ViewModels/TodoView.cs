using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TodoAPI.Models;

namespace TodoAPI.ViewModels
{
    public class TodoView
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsDone { get; set; }
        public TodoTypeView Type { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
