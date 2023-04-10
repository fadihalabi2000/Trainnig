


using NewsApiDomin.Models;
using NewsApiServies.CRUD.Interfaces;
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

        public async Task<List<Log>> GetAllAuthorsLogAsync()
        {
            return await unitOfWorkRepo.LogRepository.GetAllAuthorsLogAsync();
        }

        public async Task<List<Log>> GetAllUsersLogAsync()
        {
            return await unitOfWorkRepo.LogRepository.GetAllUsersLogAsync();
        }

  

    }
}
