using AutoMapper.Configuration.Annotations;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels.CommentViewModel;
using NewsApiDomin.ViewModels.LikeViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApiDomin.ViewModels.ArticleViewModel
{
    public class ArticleWithAuthorView
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public int ViewCount { get; set; }
        public DateTime PublishDate { get; set; } = DateTime.UtcNow;
        public DateTime UpdateDate { get; set; } = DateTime.MinValue;
        public int CategoryId { get; set; }
        public string AuthorDisplayName { get; set; } = string.Empty;
        public string ProfilePicture { get; set; } = string.Empty;
        public List<Image> Images { get; set; } = new List<Image>();

        public List<ListLikeView> Likes { get; set; } = new List<ListLikeView>();
        public List<ListCommentView> Comments { get; set; } = new List<ListCommentView>();
    }
}
