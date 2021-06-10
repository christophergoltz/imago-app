using System.Threading.Tasks;
using Imago.Models;

namespace Imago.Repository.WrappingDatabase
{
    public interface IArmorRepository : IObjectJsonRepository<ArmorSet>
    {
        Task EnsureTables();
    }
}