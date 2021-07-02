using System;

namespace ImagoApp.Models
{
    public class TableInfoModel : Util.BindableBase
    {
        private Enum.TableInfoState _state;
        private DateTime? _timeStamp;
        private int _count;
        private Enum.TableInfoType _type;

        public TableInfoModel(Enum.TableInfoType infoType)
        {
            Type = infoType;
        }

        public Enum.TableInfoType Type
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

        public Enum.TableInfoState State
        {
            get => _state;
            set => SetProperty(ref _state, value);
        }
    }
}