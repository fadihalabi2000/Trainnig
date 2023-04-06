using NewsApiDomin.Models;
using Services.CRUD.Interfaces;
using Services.Transactions.Interfaces;


namespace Services.CRUD
{
    public class UsersService : BaseCRUDService<User>, IUsersService 
    {
        private readonly IUnitOfWork unitOfWork;

        public UsersService(IUnitOfWork unitOfWork) : base(unitOfWork.UserRepository)
        {
            this.unitOfWork = unitOfWork;
        }

      
    }
}
