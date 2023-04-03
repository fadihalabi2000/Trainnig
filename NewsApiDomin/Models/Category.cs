using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApiDomin.Models
{
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty; 
        public List<Article> articles { get; set; }=new List<Article>();
        public List<FavoriteCategorie> favoriteCategories { get; set; }= new List<FavoriteCategorie>();

    }
}
