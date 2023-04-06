using NewsApiDomin.Models;
using Repositories.Interfaces;
using Services.CRUD.Interfaces;
using Services.Transactions.Interfaces;


namespace Services.CRUD
{
    public class UsersService : BaseCRUDService<User>, IUsersService 
    {
        private readonly IUnitOfWorkRepo unitOfWorkRepo;

        public UsersService(IUnitOfWorkRepo unitOfWorkRepo) : base(unitOfWorkRepo.UserRepository)
        {
            this.unitOfWorkRepo = unitOfWorkRepo;
        }

      
    }
}
