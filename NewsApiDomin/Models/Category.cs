using DataAccess.Entities.Abstractions.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApiDomin.Models
{
    public class Category: BaseNormalEntity
    {
      
        public string CategoryName { get; set; } = string.Empty; 
        public List<Article> Articles { get; set; }=new List<Article>();
        public List<FavoriteCategorie> FavoriteCategories { get; set; }= new List<FavoriteCategorie>();

    }
}
