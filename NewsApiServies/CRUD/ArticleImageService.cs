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

namespace NewsApiServies.CRUD
{
   
        public class ArticleImageService : BaseCRUDService<ArticleImage>, IArticleImageService
        {
            private readonly IUnitOfWork unitOfWork;

            public ArticleImageService(IUnitOfWork unitOfWork) : base(unitOfWork.ArticleImageRepository)
            {
                this.unitOfWork = unitOfWork;
            }


    }
    
}
