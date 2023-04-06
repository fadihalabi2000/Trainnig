using NewsApiDomin.Models;
using NewsApiServies.CRUD.Interfaces;
using Repositories.Interfaces;
using Services.CRUD;
using Services.Transactions.Interfaces;

namespace NewsApiServies.CRUD
{
    public class LogService : BaseCRUDService<Log>, ILogService
    {
        private readonly IUnitOfWorkRepo unitOfWorkRepo;

        public LogService(IUnitOfWorkRepo unitOfWorkRepo) : base(unitOfWorkRepo.LogRepository)
        {
            this.unitOfWorkRepo = unitOfWorkRepo;
        }

    }
}
