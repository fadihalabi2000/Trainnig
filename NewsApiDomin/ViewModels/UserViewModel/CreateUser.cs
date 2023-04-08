using NewsApiDomin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApiDomin.ViewModels.UserViewModel
{
    public class CreateUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    //    public string DisplayName { get; set; } = string.Empty;
        public string ProfilePicture { get; set; } = string.Empty;

    }
}
