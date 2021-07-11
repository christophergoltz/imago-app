﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using ImagoApp.Infrastructure.Entities.Template;

namespace ImagoApp.Infrastructure.Repositories
{
    public interface IWeaponTemplateRepository : IRepository<WeaponTemplateEntity>
    {
    }

    public class WeaponTemplateRepository : Repository<WeaponTemplateEntity>, IWeaponTemplateRepository
    {
        public WeaponTemplateRepository(string databaseFolder) : base(Path.Combine(databaseFolder, "ImagoApp_Wikidata.db"))
        {
        }
    }
}