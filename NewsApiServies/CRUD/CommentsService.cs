using NewsApiDomin.Models;
using Repositories.Interfaces;
using Services.CRUD.Interfaces;
using Services.Transactions.Interfaces;

namespace Services.CRUD
{
    public class CommentsService : BaseCRUDService<Comment>, ICommentsService
    {
        private readonly IUnitOfWorkRepo unitOfWorkRepo;

        public CommentsService(IUnitOfWorkRepo unitOfWorkRepo) : base(unitOfWorkRepo.CommentRepository)
        {
            this.unitOfWorkRepo = unitOfWorkRepo;
        }

     
    }
}
