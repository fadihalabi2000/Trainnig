using NewsApiDomin.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsApiServies.Pagination.Interface
{
    public interface IPaginationService<TList>
    {
        public Task<(List<TList>, PaginationMetaData)> GetPaginationAsync(int pageNumber, int pageSize ,List<TList> list);

    }
}
