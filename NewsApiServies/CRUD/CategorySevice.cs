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

namespace NewsApiServies.CRUD
{
   
    public class CategorySevice : BaseCRUDService<Category>, ICategoryService
    {
        private readonly IUnitOfWork unitOfWork;

       public CategorySevice(IUnitOfWork unitOfWork) : base(unitOfWork.CategoryRepository)
       {
            this.unitOfWork = unitOfWork;
       }


    }
}
