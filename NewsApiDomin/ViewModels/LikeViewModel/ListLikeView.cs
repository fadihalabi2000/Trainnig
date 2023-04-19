using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApiDomin.ViewModels.LikeViewModel
{
    public class ListLikeView
    {
        public int ArticleId { get; set; }
        public int UserId { get; set; }
        public string UserDisplayName { get; set; } = string.Empty;
        public string UserProfilePicture { get; set; } = string.Empty;
    }
}
