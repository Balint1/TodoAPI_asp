using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace TodoAPI.ViewModels
{
    public class TodoCategoryView
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [Required]
        public Int32 Argb { get; set; }
    }
}
