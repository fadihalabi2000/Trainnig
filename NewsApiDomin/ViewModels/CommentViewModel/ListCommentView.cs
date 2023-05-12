using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApiDomin.ViewModels.CommentViewModel
{
    public class ListCommentView
    {
        public int Id { get; set; }
        public int ArticleId { get; set; }
        public int UserId { get; set; }
        public string CommentText { get; set; } = string.Empty;
        public string UserDisplayName { get; set; } = string.Empty;
        public string UserProfilePicture { get; set; } = string.Empty;
    }
}
