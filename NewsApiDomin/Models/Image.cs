using DataAccess.Entities.Abstractions.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApiDomin.Models
{
    public class Image : BaseNormalEntity
    {
        public string ImageUrl { get; set; } = string.Empty;
        public string ImageDescription { get; set; } = string.Empty;

        public int ArticleId { get; set; }

        public  Article Article { get; set; } = new Article();
    }
}
