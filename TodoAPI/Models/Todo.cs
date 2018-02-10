using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace TodoAPI.Models
{
    public class Todo
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public bool IsDone { get; set; }
        public TodoType Type { get; set; }
        public DateTime CreationDate { get; set; }
        public bool Archived { get; set; }
        public bool Deleted { get; set; }
    }
}
