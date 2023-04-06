using NewsApiDomin.Models;
using NewsApiServies.CRUD.Interfaces;
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
        private readonly IUnitOfWork unitOfWork;

        public LikeService(IUnitOfWork unitOfWork) : base(unitOfWork.LikeRepository)
        {
            this.unitOfWork = unitOfWork;
        }

    }
}
