using DataAccess.Entities.Abstractions.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApiDomin.Models
{
    public class User : BaseNormalEntity
    {
        
        public string FirstName { get; set; }=string.Empty;
        public string LastName { get; set; }=string.Empty;
        public string Email { get; set; }=string.Empty;
        public string Password { get; set; }=string.Empty;
        public string ProfilePicture { get; set; } = string.Empty;
        public List<Like> likes { get; set; } = new List<Like>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<FavoriteCategorie> FavoriteCategories { get; set; } = new List<FavoriteCategorie>();
        public List<Log> Logs { get; set; } = new List<Log>();


    }
}
