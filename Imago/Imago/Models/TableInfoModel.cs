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
        private DateTime? _timeStamp;
        private int _count;
        private TableInfoType _type;

        public TableInfoModel(TableInfoType infoType)
        {
            Type = infoType;
        }

        public TableInfoType Type
        {
            get => _type;
            set => SetProperty(ref _type, value);
        }

        public DateTime? TimeStamp
        {
            get => _timeStamp;
            set => SetProperty(ref _timeStamp, value);
        }

        public int Count
        {
            get => _count;
            set => SetProperty(ref _count, value);
        }

        public TableInfoState State
        {
            get => _state;
            set => SetProperty(ref _state, value);
        }
    }
}