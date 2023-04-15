using NewsApiDomin.Models;
using NewsApiDomin.ViewModels.UserViewModel;
using NewsApiRepositories.UnitOfWorkRepository.Interface;
using Repositories.Interfaces;
using Services.CRUD.Interfaces;


namespace Services.CRUD
{
    public class UsersService : BaseCRUDService<User>, IUsersService 
    {
        private readonly IUnitOfWorkRepo unitOfWorkRepo;

        public UsersService(IUnitOfWorkRepo unitOfWorkRepo) : base(unitOfWorkRepo.UserRepository)
        {
            this.unitOfWorkRepo = unitOfWorkRepo;
        }

        public async Task<User> CheckDisplayName(string displayName)
        {
           return await unitOfWorkRepo.UserRepository.CheckDisplayName(displayName);
        }

        public new async Task<User> GetByIdAsync(int id)
        {
            return await unitOfWorkRepo.UserRepository.GetByIdAsync(id);
           
        }


    }
}
