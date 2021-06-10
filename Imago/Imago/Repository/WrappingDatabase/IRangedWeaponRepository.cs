using System.Threading.Tasks;
using Imago.Models;

namespace Imago.Repository.WrappingDatabase
{
    public interface IRangedWeaponRepository : IObjectJsonRepository<Weapon>
    {
        Task EnsureTables();
    }
}