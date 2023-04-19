﻿

using NewsApiDomin.Models;
using NewsApiDomin.ViewModels;
using NewsApiDomin.ViewModels.ArticleViewModel;

namespace Services.CRUD.Interfaces
{
    public interface IArticleService : IBaseCRUDService<Article>
    {
        Task<(List<ArticleWithAuthorView>, bool isCompleted)> GetArticlesAsync(List<Article>  articles );
    }
}
