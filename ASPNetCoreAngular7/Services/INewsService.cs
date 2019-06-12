using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ASPNetCoreAngular7
{
    public interface INewsService
    {
        Task<int[]> GetIdsAsync();
        Task<NewsArticle> GetItemByIdAsync(int id);
    }
}
