using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TodoAPI.ViewModels
{
    public class LoginView
    {
        [Required, EmailAddress, MaxLength(256)]
        public string Email { get; set; }
        [Required, MinLength(6), MaxLength(50), DataType(DataType.Password)]
        public string Password { get; set; }
        [Required]
        public bool RememberMe { get; internal set; }
    }
}
