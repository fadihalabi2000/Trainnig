using DataAccess.Entities.Abstractions.Classes;
using System.Text.Json.Serialization;

namespace NewsApiDomin.Models
{
    public class User : BaseNormalEntity
    {
        
        public string FirstName { get; set; }=string.Empty;
        public string LastName { get; set; }=string.Empty;
        public string Email { get; set; }=string.Empty;
        public string Password { get; set; }=string.Empty;
        public string ProfilePicture { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public List<Like> likes { get; set; } = new List<Like>();
        public List<Comment> Comments { get; set; } = new List<Comment>();
        public List<Log> Logs { get; set; } = new List<Log>();
        public List<Author>  Authors { get; set; } = new List<Author>();
        public List<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();


    }
}
