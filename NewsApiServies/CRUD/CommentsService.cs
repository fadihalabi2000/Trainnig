using NewsApiDomin.Models;
using NewsApiRepositories.UnitOfWorkRepository.Interface;
using Repositories.Interfaces;
using Services.CRUD.Interfaces;

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
