

using Microsoft.Extensions.Logging;
using NewsApiDomin.Enums;
using NewsApiDomin.Models;
using NewsApiRepositories.UnitOfWorkRepository.Interface;

namespace Services.MyLogger
{
    public class MyLogger : IMyLogger
    {
        private readonly IUnitOfWorkRepo unitOfWorkRepo;

        public MyLogger(IUnitOfWorkRepo unitOfWorkRepo) 
        {
            this.unitOfWorkRepo = unitOfWorkRepo;
        }

        public async Task log(string message, String messagetype, int id, string role)
        {      
            var logLevel=(NewsApiDomin.Enum.LogLevel)Enum.Parse(typeof(NewsApiDomin.Enum.LogLevel), messagetype);
            var log = new Log {  logLevel = logLevel, Content = message, };
            //user
            if (role== Roles.User.ToString())
            {
                log.AuthorId=int.MaxValue;
                log.UserId = id;
               await unitOfWorkRepo.LogRepository.AddAsync(log);
               await unitOfWorkRepo.CommitAsync();
            }
            //Author
            else if(role== Roles.Author.ToString())
            {
                log.AuthorId = id;
                log.UserId = int.MaxValue;
               await unitOfWorkRepo.LogRepository.AddAsync(log);
              await  unitOfWorkRepo.CommitAsync();
            }
  
        }
    }
}
