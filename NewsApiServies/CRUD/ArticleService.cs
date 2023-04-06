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
        private readonly IUnitOfWorkRepo unitOfWorkRepo;

        public ArticleService(IUnitOfWorkRepo unitOfWorkRepo) : base(unitOfWorkRepo.ArticleRepository)
        {
            this.unitOfWorkRepo = unitOfWorkRepo;
        }

       
    }
}
