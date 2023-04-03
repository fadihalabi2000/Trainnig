using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApiDomin.Models
{
    public class Article
    {
        public int ArticleId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int ViewCount { get; set; }

        public bool IsDeleted { get; set; }
        public DateTime PublishDate { get; set; }=DateTime.MinValue;
        public DateTime UpdateDate { get; set; }=DateTime.MinValue;

        public int AuthorId { get; set; }
        public int CategoryId { get; set; }

        public Author Author { get; set; } = new Author();  
        public Category Category { get; set; } = new Category();

        public List<Like> likes{ get; set; } =new List<Like>();
        public List<Comment> comments{ get; set; } =new List<Comment>();
        public List<ArticleImage> articleImages{ get; set; } =new List<ArticleImage>();
       
    }
}
