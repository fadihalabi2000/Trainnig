using NewsApiDomin.Models;
using Services.CRUD;
using NewsApiServies.CRUD.Interfaces;
using Repositories.Interfaces;
using NewsApiRepositories.UnitOfWorkRepository.Interface;

namespace NewsApiServies.CRUD
{

    public class AuthorService : BaseCRUDService<Author>, IAuthorService
    {
        private readonly IUnitOfWorkRepo unitOfWorkRepo;

        public AuthorService(IUnitOfWorkRepo unitOfWorkRepo) : base(unitOfWorkRepo.AuthorRepository)
        {
            this.unitOfWorkRepo = unitOfWorkRepo;
        }


    }
}
