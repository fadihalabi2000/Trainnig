

using DataAccess.Entities.Abstractions.Classes;

namespace  NewsApiDomin.Models
{

    public class RefreshToken: BaseNormalEntity
    {

        public int? AuthorId { get; set; } 
        public int? UserId { get; set; } 
        public string? Token { get; set; }
        public DateTime ExpiresOn { get; set; }
        public bool IsExpired  => DateTime.UtcNow >= ExpiresOn;
        public DateTime CreatedOn { get; set; }
        public DateTime? RevokedOn { get; set; }
        public bool IsActive => RevokedOn == null && !IsExpired;
        public Author? Author { get; set; }
        public User? User { get; set; }
    }
}