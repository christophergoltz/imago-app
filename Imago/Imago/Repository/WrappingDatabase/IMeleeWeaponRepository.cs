using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Imago.Models;

namespace Imago.Repository.WrappingDatabase
{
    public interface IMeleeWeaponRepository : IObjectJsonRepository<Weapon>
    {
        Task EnsureTables();
    }
}
