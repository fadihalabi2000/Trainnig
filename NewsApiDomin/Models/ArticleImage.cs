using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApiDomin.Models
{
    public class ArticleImage
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public int ImageId { get; set; }

        public Article Article { get; set; } = new Article();
        public Image Image { get; set; } = new Image();

    }
}
