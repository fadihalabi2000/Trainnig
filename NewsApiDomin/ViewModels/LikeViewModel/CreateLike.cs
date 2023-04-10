using NewsApiDomin.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApiDomin.ViewModels.LikeViewModel
{
    public class CreateLike
    {
        [Required]
        public int ArticleId { get; set; }
        [Required]
        public int UserId { get; set; }

    }
}
