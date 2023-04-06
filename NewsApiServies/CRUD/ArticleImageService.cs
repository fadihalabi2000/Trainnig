using NewsApiDomin.Models;
using Services.CRUD.Interfaces;
using Services.CRUD;
using Services.Transactions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Repositories;
using Repositories.Interfaces;

namespace NewsApiServies.CRUD
{
   
        public class ArticleImageService : BaseCRUDService<ArticleImage>, IArticleImageService
        {
        private readonly IUnitOfWorkRepo unitOfWorkRepo;

        public ArticleImageService(IUnitOfWorkRepo unitOfWorkRepo) : base(unitOfWorkRepo.ArticleImageRepository)
            {
            this.unitOfWorkRepo = unitOfWorkRepo;
        }


    }
    
}
