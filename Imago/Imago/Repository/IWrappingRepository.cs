using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Imago.Models;
using Imago.Models.Entity;

namespace Imago.Repository
{
    public interface IWrappingRepository<TModel, TEntity>
        where TModel : class, new()
        where TEntity : IJsonValueWrapper<TModel>, new()
    {
        Task<List<TModel>> GetAllItemsAsync();
        Task DeleteAllItems();
        Task AddAllItems(IEnumerable<TModel> items);
        DateTime GetLastChangedDate();
        Task<int> GetItemsCount();
    }
}