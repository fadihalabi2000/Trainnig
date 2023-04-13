


using NewsApiDomin.Models;
using NewsApiRepositories.UnitOfWorkRepository.Interface;
using NewsApiServies.CRUD.Interfaces;
using Services.CRUD;

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

        public async Task<List<Log>> GetLogAuthorByIdAsync(int id)
        {
            return await unitOfWorkRepo.LogRepository.GetLogAuthorByIdAsync(id);
        }

        public async Task<List<Log>> GetLogUserByIdAsync(int id)
        {
            return await unitOfWorkRepo.LogRepository.GetLogUserByIdAsync(id);
        }

        public async Task<List<Log>> GetAllUsersLogAsync()
        {
            return await unitOfWorkRepo.LogRepository.GetAllUsersLogAsync();
        }


  

    }
}
