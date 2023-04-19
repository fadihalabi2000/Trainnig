using NewsApiDomin.Models;
using Services.CRUD.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApiServies.CRUD.Interfaces
{
    public interface IImageService:IBaseCRUDService<Image>
    {
        Task<List<Image>> GetAllByIdArticleAsync(int articleId);
    }
}
