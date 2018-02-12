using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoAPI.Models
{
    public class Todo
    {
        public Todo()
        {

        }
        public Todo(int id, string title,TodoCategory todoCategory,bool isDone = false)
        {
            Id = id;
            Title = title;
            IsDone = isDone;
            Type = todoCategory;
            Deleted = false;
            Archived = false;
        }
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsDone { get; set; }
        public TodoCategory Type { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Archived { get; set; }
        public bool Deleted { get; set; }
    }
}
