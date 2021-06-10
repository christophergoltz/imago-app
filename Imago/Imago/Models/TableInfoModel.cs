using System;
using System.Collections.Generic;
using System.Text;
using Imago.Models.Enum;
using Imago.Util;

namespace Imago.Models
{
    public class TableInfoModel : BindableBase
    {
        private TableInfoState _state;

        public TableInfoModel()
        {

        }

        public TableInfoType Type { get; set; }
        public DateTime? TimeStamp { get; set; }
        public int Count { get; set; }

        public TableInfoState State
        {
            get => _state;
            set => SetProperty(ref _state, value);
        }
    }
}
