using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using TodoAPI.Models;

namespace TodoAPI.ViewModels
{
    public class TodoView
    {
        public int Id { get; set; }
        [Required]
        [StringLength(30)]
        public string Title { get; set; }
        public bool IsDone { get; set; }
        public TodoCategoryView Type { get; set; }
        public DateTime CreationDate { get; set; }
    }
}
