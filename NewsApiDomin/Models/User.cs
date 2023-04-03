using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApiDomin.Models
{
    public class User
    {
        public int UserId { get; set; }
        public string FirstName { get; set; }=string.Empty;
        public string LastName { get; set; }=string.Empty;
        public string Email { get; set; }=string.Empty;
        public string Password { get; set; }=string.Empty;
        public string ProfilePicture { get; set; } = string.Empty;
        public List<Like> likes { get; set; } = new List<Like>();
        public List<Comment> comments { get; set; } = new List<Comment>();
        public List<FavoriteCategorie> favoriteCategories { get; set; } = new List<FavoriteCategorie>();
        public List<Log> logs { get; set; } = new List<Log>();


    }
}
