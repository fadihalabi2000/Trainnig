using DataAccess.Entities.Abstractions.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApiDomin.Models
{
    public class Article: BaseNormalEntity
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int ViewCount { get; set; }
        public DateTime PublishDate { get; set; }=DateTime.MinValue;
        public DateTime UpdateDate { get; set; }=DateTime.MinValue;

        public int AuthorId { get; set; }
        public int CategoryId { get; set; }

        public Author Author { get; set; } = new Author();  
        public Category Category { get; set; } = new Category();

        public List<Like> Likes{ get; set; } =new List<Like>();
        public List<Comment> Comments{ get; set; } =new List<Comment>();
        public List<ArticleImage> ArticleImages{ get; set; } =new List<ArticleImage>();
       
    }
}
