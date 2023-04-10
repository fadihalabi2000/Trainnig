using DataAccess.Entities.Abstractions.Classes;
using NewsApiDomin.Enum;
using System.Diagnostics.CodeAnalysis;

namespace NewsApiDomin.Models
{
    public class Log : BaseNormalEntity
    {
        public  LogLevel logLevel { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }= DateTime.UtcNow;
      
        public int AuthorId { get; set; }=int.MaxValue;
       
        public int UserId { get; set; } =int.MaxValue;

        public Author Author { get; set; }=new Author();
        public User User { get; set; }=new User();

      
    }
}
