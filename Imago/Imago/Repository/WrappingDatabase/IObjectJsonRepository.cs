using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Imago.Repository.WrappingDatabase
{
    public interface IObjectJsonRepository<TModel>
        where TModel : class, new()
    {
        DateTime GetLastChangedDate();
        Task<int> GetItemsCount();
        Task<List<TModel>> GetAllItemsAsync();
        Task DeleteAllItems();
        Task<int> AddAllItems(IEnumerable<TModel> items);
    }
}