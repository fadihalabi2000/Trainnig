using NewsApiDomin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApiDomin.ViewModels.AuthorViewModel
{
    public class AuthorViewModel
    { 
        public int Id { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string ProfilePicture { get; set; } = string.Empty;
        public string AccountStats { get; set; } = string.Empty;
        public List<Article> Articles { get; set; } = new List<Article>();
    }
}
