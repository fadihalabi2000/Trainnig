using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApiDomin.Models
{
    public class Like
    {
        public int LikeId { get; set; }    
        public DateTime LikeDate { get; set; }=DateTime.MinValue;
        public int ArticleId { get; set; }
        public int UserId { get; set; }
        public Article Article { get; set; } =new Article();
        public User User { get; set; } =new User();

    }
}
