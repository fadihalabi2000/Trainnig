using AutoMapper;
using NewsApiDomin.Models;
using NewsApiDomin.ViewModels.ArticleViewModel;
using NewsApiDomin.ViewModels.AuthorViewModel;
using NewsApiDomin.ViewModels.CategoryViewModel;
using NewsApiDomin.ViewModels.CommentViewModel;
using NewsApiDomin.ViewModels.ImageViewModel;
using NewsApiDomin.ViewModels.LikeViewModel;
using NewsApiDomin.ViewModels.LogViewModel.AuthorLogViewModel;
using NewsApiDomin.ViewModels.LogViewModel.UserLogViewModel;
using NewsApiDomin.ViewModels.UserViewModel;

namespace NewsApiDomin.MappingProfile
{
    public class MappingProfile:Profile
    {
        public MappingProfile()
        {
            CreateMap<Category, CategoryView>().ReverseMap();
            CreateMap<CreateCategory, Category>();
            CreateMap<UpdateCategory, Category>();
            CreateMap<Article, ArticlView>().ReverseMap();
            CreateMap<CreateArticle, Article>();
            CreateMap<UpdateArticle, Article>().ReverseMap();
            CreateMap<Author, AuthorView>().ReverseMap();
            CreateMap<UpdateAuthor, Author>().ReverseMap();
            CreateMap<CreateAuthor, Author>().ReverseMap();
            CreateMap<Comment, CommentView>();
            CreateMap<CreateComment, Comment>();
            CreateMap<UpdateComment, Comment>();
            CreateMap<Image, ImageView>();
            CreateMap<CreateImage, Image>();
            CreateMap<UpdateImage, Image>();
            CreateMap<Like, LikeView>();
            CreateMap<CreateLike, Like>();
            CreateMap<User, UserView>();
            CreateMap<UpdateUser, User>().ReverseMap();
            CreateMap<CreateUser, User>().ReverseMap();
            CreateMap<User, UserWithoutLog>();
            CreateMap<Log, UserLogView>();
            CreateMap<CreateUserLog, Log>();
            CreateMap<Log, AuthorLogView>();
            CreateMap<CreateAuthorLog, Log>();

        }
    }
}
