using NewsApiDomin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApiDomin.ViewModels.ArticleViewModel
{
    public class CreateOrUpdateArticle
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int ViewCount { get; set; }
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public List<Image> ArticleImages { get; set; } = new List<Image>();
    }
}
