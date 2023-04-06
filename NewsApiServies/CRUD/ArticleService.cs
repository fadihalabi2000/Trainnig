using DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using NewsApiDomin.Models;
using Repositories.Interfaces;
using Services.CRUD.Interfaces;
using Services.Transactions;
using Services.Transactions.Interfaces;
using System.Threading.Tasks;

namespace Services.CRUD
{
    public class ArticleService : BaseCRUDService<Article>, IArticleService
    {
        private readonly IUnitOfWork unitOfWork;

        public ArticleService(IUnitOfWork unitOfWork) : base(unitOfWork.ArticleRepository)
        {
            this.unitOfWork = unitOfWork;
        }

       
    }
}
