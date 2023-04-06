using NewsApiDomin.Models;
using NewsApiServies.CRUD.Interfaces;
using Services.CRUD;
using Services.Transactions.Interfaces;

namespace NewsApiServies.CRUD
{
    public class LogService : BaseCRUDService<Log>, ILogService
    {
        private readonly IUnitOfWork unitOfWork;

        public LogService(IUnitOfWork unitOfWork) : base(unitOfWork.LogRepository)
        {
            this.unitOfWork = unitOfWork;
        }

    }
}
