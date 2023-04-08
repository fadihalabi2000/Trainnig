using DataAccess.Entities.Abstractions.Classes;
using NewsApiDomin.Enum;


namespace NewsApiDomin.Models
{
    public class Log : BaseNormalEntity
    {
        public  LogLevel logLevel { get; set; }
        public string Content { get; set; } = string.Empty;
        public DateTime DateCreated { get; set; }= DateTime.UtcNow;
        public int AuthorId { get; set; } = 0;
        public int UserId { get; set; } = 0;

        public Author Author { get; set; }=new Author();
        public User User { get; set; }=new User();

      
    }
}
