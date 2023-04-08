using NewsApiDomin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApiDomin.ViewModels.CommentViewModel
{
    public class CreateComment
    {
        public string CommentText { get; set; } = string.Empty;
        public int ArticleId { get; set; }
        public int UserId { get; set; }
    
    }
}
