

using NewsApiDomin.Models;
using NewsApiRepositories.UnitOfWorkRepository.Interface;
using NewsApiServies.CRUD.Interfaces;
using Services.CRUD;

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
