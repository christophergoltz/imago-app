using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models.Enum;
using SQLite;

namespace Imago.Database
{
    public class TableInfoEntity
    {
        public TableInfoEntity()
        {
            
        }

        [PrimaryKey]
        public TableInfoType Type { get; set; }
        public DateTime? TimeStamp { get; set; }
    }
}
