using NewsApiDomin.Models;
using NewsApiServies.CRUD.Interfaces;
using Repositories.Interfaces;
using Services.CRUD;
using Services.Transactions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApiServies.CRUD
{
  
    public class LikeService : BaseCRUDService<Like>, ILikeService
    {
        private readonly IUnitOfWorkRepo unitOfWorkRepo;

        public LikeService(IUnitOfWorkRepo unitOfWorkRepo) : base(unitOfWorkRepo.LikeRepository)
        {
            this.unitOfWorkRepo = unitOfWorkRepo;
        }

    }
}
