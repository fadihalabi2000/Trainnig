using NewsApiDomin.Models;
using Services.CRUD.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApiServies.CRUD.Interfaces
{
    public interface ICategoryService:IBaseCRUDService<Category>
    {
        Task<Category> CheckCategoryName(string categoryName);
    }
}
