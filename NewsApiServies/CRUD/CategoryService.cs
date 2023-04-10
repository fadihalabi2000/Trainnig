

using NewsApiDomin.Models;
using NewsApiServies.CRUD.Interfaces;
using Services.CRUD;
using Services.Transactions.Interfaces;

namespace NewsApiServies.CRUD
{
   
    public class CategoryService : BaseCRUDService<Category>, ICategoryService
    {
        private readonly IUnitOfWorkRepo unitOfWorkRepo;

        public CategoryService(IUnitOfWorkRepo unitOfWorkRepo) : base(unitOfWorkRepo.CategoryRepository)
        {
            this.unitOfWorkRepo = unitOfWorkRepo;
        }


    }
}
