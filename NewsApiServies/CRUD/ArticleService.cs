


using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels;
using NewsApiDomin.ViewModels.ArticleViewModel;
using NewsApiDomin.ViewModels.CommentViewModel;
using NewsApiDomin.ViewModels.LikeViewModel;
using NewsApiRepositories.UnitOfWorkRepository.Interface;
using NewsApiServies.Auth.ClassStatic;
using Services.CRUD.Interfaces;
using Services.Transactions;
using Services.Transactions.Interfaces;

namespace Services.CRUD
{
    public class ArticleService : BaseCRUDService<Article>, IArticleService
    {
        private readonly IUnitOfWorkRepo unitOfWorkRepo;
       

        public ArticleService(IUnitOfWorkRepo unitOfWorkRepo) : base(unitOfWorkRepo.ArticleRepository)
        {
            this.unitOfWorkRepo = unitOfWorkRepo;
           
        }
    
        public async Task<(List<ArticleWithAuthorView>,  bool isCompleted)> GetArticlesAsync(List<Article> articles)
        {
            try
            {            
             List<Author> authors = await unitOfWorkRepo.AuthorRepository.GetAllAsync();
             List<User> users = await unitOfWorkRepo.UserRepository.GetAllAsync();
             List<Like> likes = await unitOfWorkRepo.LikeRepository.GetAllAsync();
             List<Comment> comments = await unitOfWorkRepo.CommentRepository.GetAllAsync();

             List<ListLikeView> listLikeViews = likes.Join(
                                                users, like => like.UserId, user => user.Id,
                                                (like, user) => 
                                                new ListLikeView {
                                                    id=like.Id,
                                                UserId = user.Id,
                                                ArticleId = like.ArticleId,
                                                UserDisplayName = user.DisplayName,
                                                UserProfilePicture = user.ProfilePicture
                                                }).ToList();
             List<ListCommentView> listCommentViews = comments.Join(
                                                      users, comment =>comment.UserId, user => user.Id, 
                                                      (comment, user) =>
                                                      new ListCommentView 
                                                      {Id=comment.Id,
                                                          UserId = user.Id, ArticleId = comment.ArticleId,
                                                      UserDisplayName = user.DisplayName,
                                                      UserProfilePicture = user.ProfilePicture,
                                                      CommentText = comment.CommentText
                                                      }).ToList();
             List<ArticleWithAuthorView> listArticles = articles.Join(
                                                        authors, article => article.AuthorId, 
                                                        author => author.Id, (article, author) => 
                                                        new ArticleWithAuthorView {
                                                        Id = article.Id,
                                                        CategoryId = article.CategoryId,
                                                        Title = article.Title,
                                                        Content = article.Content, 
                                                        ViewCount = article.ViewCount, 
                                                        Images = article.Images,
                                                        PublishDate = article.PublishDate,
                                                        UpdateDate = article.UpdateDate,
                                                        AuthorDisplayName = author.DisplayName,
                                                        ProfilePicture = author.ProfilePicture 
                                                        }).ToList();
             List<ArticleWithAuthorView> articlesWithAuthorsViews = listArticles.GroupJoin(
                                                                    listLikeViews, a => a.Id,
                                                                    l => l.ArticleId, (a, l) => 
                                                                    new { Article = a, Likes = l })
                                                                    .GroupJoin(listCommentViews, 
                                                                     al => al.Article.Id, c => c.ArticleId,
                                                                     (al, c) =>
                                                                     new { ArticleLikes = al, Comments = c })
                                                                    .Select(r => new ArticleWithAuthorView
                                                                    {
                                                                    Id = r.ArticleLikes.Article.Id,
                                                                    CategoryId = r.ArticleLikes.Article.CategoryId,
                                                                    ViewCount = r.ArticleLikes.Article.ViewCount,
                                                                    Title = r.ArticleLikes.Article.Title,
                                                                    Content = r.ArticleLikes.Article.Content,
                                                                    PublishDate = r.ArticleLikes.Article.PublishDate,
                                                                    UpdateDate = r.ArticleLikes.Article.UpdateDate,
                                                                    AuthorDisplayName = r.ArticleLikes.Article.AuthorDisplayName,
                                                                    ProfilePicture = r.ArticleLikes.Article.ProfilePicture,
                                                                    Comments = r.Comments.Except(r.ArticleLikes.Article.Comments)
                                                                    .ToList(),
                                                                    Images = r.ArticleLikes.Article.Images,
                                                                    Likes = r.ArticleLikes.Likes.Except(r.ArticleLikes.Article.Likes)
                                                                    .ToList()
                                                                    })
                                                                    .ToList();
             return (articlesWithAuthorsViews,  true);
              

            }
            catch (Exception)
            {
                return (new List<ArticleWithAuthorView> { }, false);
            }


        }

        public async Task<List<Image>> UploadImage(IFormFile imageFile, String contentPath)
        {
            var path = Path.Combine(contentPath, "Uploads");
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }

            // Check the allowed extenstions
            var ext = Path.GetExtension(imageFile.FileName);
            var allowedExtensions = new string[] { ".jpg", ".png", ".jpeg" };
            
            string uniqueString = Guid.NewGuid().ToString();
            // we are trying to create a unique filename here
            var newFileName = uniqueString + ext;
            var fileWithPath = Path.Combine(path, newFileName);
            var stream = new FileStream(fileWithPath, FileMode.Create);
            imageFile.CopyTo(stream);
            stream.Close();
            var imageEntity = new List<Image>
            {
              new Image{
                ImageDescription = newFileName,
                ImageUrl = fileWithPath,
              }

            };

            return imageEntity;
        }
    }
}
