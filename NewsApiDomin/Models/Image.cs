using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApiDomin.Models
{
    public class Image
    {
        public int ImageId { get; set; }
        public string ImageUrl { get; set; } = string.Empty;
        public string ImageDescription { get; set; } = string.Empty;

        public List<ArticleImage>articleImages { get; set; }=new List<ArticleImage>();
    }
}
