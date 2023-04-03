using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApiDomin.Models
{
    public class Author
    {
        public int AuthorId { get; set; }
        public string AuthorName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;
        public string ProfilePicture { get; set; } = string.Empty;
        public string AccountStats { get; set; } = string.Empty;
        public List<Article> articles { get; set; } = new List<Article>();
        public List<Log> logs { get; set; } = new List<Log>();
    }
}
