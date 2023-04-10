


using NewsApiDomin.Models;
using Services.CRUD.Interfaces;
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

       
    }
}
