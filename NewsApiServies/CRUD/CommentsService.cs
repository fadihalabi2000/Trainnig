using NewsApiDomin.Models;
using Services.CRUD.Interfaces;
using Services.Transactions.Interfaces;

namespace Services.CRUD
{
    public class CommentsService : BaseCRUDService<Comment>, ICommentsService
    {
        private readonly IUnitOfWork unitOfWork;

        public CommentsService(IUnitOfWork unitOfWork) : base(unitOfWork.CommentRepository)
        {
            this.unitOfWork = unitOfWork;
            
        }

     
    }
}
