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
            List<User>  users = await unitOfWorkRepo.UserRepository.GetAllAsync();
            return await Task.Run(() => users.FirstOrDefault(u => u.DisplayName == displayName)!);
        }

        public async Task<User> CheckNameAndEmail(string displayName, string email)
        {
            List<User> users = await unitOfWorkRepo.UserRepository.GetAllAsync();
            return await Task.Run(() => users.FirstOrDefault(u => u.DisplayName == displayName && u.Email == email)!);
        }

        public async Task<User> UserAuth(Login userLogin)
        {
            List<User> users = await unitOfWorkRepo.UserRepository.GetAllAsync();
            return await Task.Run(() => users.FirstOrDefault(u => u.Password == userLogin.Password && u.Email == userLogin.Email)!);
        }
    }
}
