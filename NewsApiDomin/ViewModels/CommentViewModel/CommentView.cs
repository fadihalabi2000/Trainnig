using NewsApiDomin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApiDomin.ViewModels.CommentViewModel
{
    public class CommentView
    { 
        public int Id { get; set; }
        public string CommentText { get; set; } = string.Empty;
        public DateTime CommentDate { get; set; } = DateTime.UtcNow;
        public int ArticleId { get; set; }
        public int UserId { get; set; }
      
    }
}
