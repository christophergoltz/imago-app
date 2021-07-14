using System;
using System.Collections.Generic;
using System.Text;

namespace ImagoApp.Application.Models
{
    public class WeightItemModel : ItemBaseModel
    {
        private int _loadValue;

        public int LoadValue
        {
            get => _loadValue;
            set => SetProperty(ref _loadValue, value);
        }

        public WeightItemModel()
        {

        }

        public WeightItemModel(int loadValue, string name) : base(name)
        {
            LoadValue = loadValue;
        }
    }
}
