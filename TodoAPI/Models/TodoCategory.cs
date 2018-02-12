using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace TodoAPI.Models
{
    public class TodoCategory
    {
        public TodoCategory()
        {

        }
        public TodoCategory(int id, string name, int argb)
        {
            Name = name;
            Id = id;
            Argb = argb;
        }
        public int Id { get; set; }
        
        public string  Name { get; set; }
        [NotMapped]
        public System.Drawing.Color Color { get; set; }

        public Int32 Argb
        {
            get
            {
                return Color.ToArgb();
            }
            set
            {
                Color = System.Drawing.Color.FromArgb(value);
            }
        }
    }
}
