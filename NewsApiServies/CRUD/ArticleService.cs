


using NewsApiDomin.Models;
using NewsApiRepositories.UnitOfWorkRepository.Interface;
using Services.CRUD.Interfaces;

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
