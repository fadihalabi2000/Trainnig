


using Microsoft.EntityFrameworkCore;
using NewsApiDomin.Models;
using NewsApiRepositories.UnitOfWorkRepository.Interface;
using NewsApiServies.CRUD.Interfaces;
using Services.CRUD;
using System.Collections.Generic;

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
            List<Log> logs=await unitOfWorkRepo.LogRepository.GetAllAsync();
            return await Task.Run(() => logs.Where(l => l.UserId == null).OrderByDescending(l => l.DateCreated).ToList());
        }

        public async Task<List<Log>> GetLogAuthorByIdAsync(int id)
        {
            List<Log> logs = await unitOfWorkRepo.LogRepository.GetAllAsync();
            return await Task.Run(() => logs.Where(l => l.AuthorId == id).OrderByDescending(l => l.DateCreated).ToList());
        }

        public async Task<List<Log>> GetLogUserByIdAsync(int id)
        {
            List<Log> logs = await unitOfWorkRepo.LogRepository.GetAllAsync();
            return await Task.Run(() => logs.Where(l => l.UserId == id).OrderByDescending(l => l.DateCreated).ToList());
        }

        public async Task<List<Log>> GetAllUsersLogAsync()
        {
            List<Log> logs = await unitOfWorkRepo.LogRepository.GetAllAsync();
            return await Task.Run(() => logs.Where(l => l.AuthorId == null).OrderByDescending(l => l.DateCreated).ToList());
        }

    }
}
