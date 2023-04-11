using NewsApiDomin.Models;
using Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApiRepositories.Interfaces
{
    public interface ILogRepository: IBaseRepository<Log>
    {
        Task<List<Log>> GetAllUsersLogAsync();
        Task<List<Log>> GetAllAuthorsLogAsync();

        Task<List<Log>> GetLogAuthorByIdAsync(int id);
        Task<List<Log>> GetLogUserByIdAsync(int id);


    }
}
