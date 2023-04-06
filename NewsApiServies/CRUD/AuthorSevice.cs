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
   
       public class AuthorSevice : BaseCRUDService<Author>, IAuthorService
    {
        private readonly IUnitOfWork unitOfWork;

        public AuthorSevice(IUnitOfWork unitOfWork) : base(unitOfWork.AuthorRepository)
        {
            this.unitOfWork = unitOfWork;
        }


    }
}
