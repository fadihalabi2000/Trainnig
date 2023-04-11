using DataAccess.Entities.Abstractions.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApiDomin.Models
{
    public class Comment: BaseNormalEntity
    {

        public string CommentText { get; set; } = string.Empty;
        public DateTime CommentDate { get; set; } = DateTime.UtcNow;
        public int ArticleId { get; set; }
        public int UserId { get; set; }
        //public Article Article { get; set; } 
        //public User User { get; set; }

    }
}
