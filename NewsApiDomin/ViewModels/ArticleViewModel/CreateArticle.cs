using NewsApiDomin.Models;
using NewsApiDomin.ViewModels.ImageViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApiDomin.ViewModels.ArticleViewModel
{
    public class CreateArticle
    {
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int ViewCount { get; set; }
        public int AuthorId { get; set; }
        public int CategoryId { get; set; }
        public List<CreateImage> Images { get; set; } = new List<CreateImage>();
    }
}
