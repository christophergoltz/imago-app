﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImagoApp.Infrastructure.Entities.Template;

namespace ImagoApp.Infrastructure.Repositories
{
    public interface IArmorTemplateRepository : IRepository<ArmorPartTemplateEntity>
    {
    }

    public class ArmorTemplateRepository : Repository<ArmorPartTemplateEntity>, IArmorTemplateRepository
    {
        public ArmorTemplateRepository(string databaseFolder) : base(databaseFolder)
        {
        }
    }
}
