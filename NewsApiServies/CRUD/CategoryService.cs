using NewsApiDomin.Models;
using Services.CRUD.Interfaces;
using Services.CRUD;
using Services.Transactions.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NewsApiServies.CRUD.Interfaces;
using Repositories.Interfaces;

namespace NewsApiServies.CRUD
{
   
    public class CategoryService : BaseCRUDService<Category>, ICategoryService
    {
        private readonly IUnitOfWorkRepo unitOfWorkRepo;

        public CategoryService(IUnitOfWorkRepo unitOfWorkRepo) : base(unitOfWorkRepo.CategoryRepository)
       {
            this.unitOfWorkRepo = unitOfWorkRepo;
        }


    }
}
