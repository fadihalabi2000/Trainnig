using NewsApiDomin.ViewModels;
using NewsApiServies.Pagination.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApiServies.Pagination
{
    public class PaginationService<TList> : IPaginationService<TList>
    {
        public async Task<(List<TList>, PaginationMetaData)> GetPaginationAsync(int pageNumber,int pageSize,List<TList> list)
        {
            {
                var totalProductsNumber = list.Count();
                var paginationMetaData = new PaginationMetaData(totalProductsNumber, pageNumber, pageSize);
                List<TList> filterProduct =list.Skip(pageSize * (pageNumber - 1)).Take(pageSize).ToList();
               
            return  await Task.Run(() => (filterProduct, paginationMetaData));
            }
        }
    }
}
