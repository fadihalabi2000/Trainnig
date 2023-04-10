using NewsApiDomin.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApiDomin.ViewModels.LikeViewModel
{
    public class LikeView
    {
        public int Id { get; set; } 
        public DateTime LikeDate { get; set; } = DateTime.UtcNow;
        public int ArticleId { get; set; }
        public int UserId { get; set; }
    
    }
}
