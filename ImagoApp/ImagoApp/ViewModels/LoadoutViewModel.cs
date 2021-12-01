using System;
using System.Collections.Generic;
using System.Linq;
using ImagoApp.Application;
using ImagoApp.Application.Models;
using ImagoApp.Application.Models.Base;
using ImagoApp.Shared.Enums;

namespace ImagoApp.ViewModels
{
    public class LoadoutViewModel : BindableBase
    {
        public event EventHandler LoadoutValueChanged;

        public LoadoutViewModel(List<DerivedAttributeModel> handicapAttributes)
        {
            Handicaps = CreateHandicaps(handicapAttributes);
        }

        private List<HandicapListViewItemViewModel> _handicaps;
        public List<HandicapListViewItemViewModel> Handicaps
        {
            get => _handicaps;
            set => SetProperty(ref _handicaps, value);
        }

        public int GetLoadoutValue()
        {
            return Handicaps.FirstOrDefault(model => model.IsChecked)?.Value?.FinalValue.GetRoundedValue() ?? 0;
        }
        
        private List<HandicapListViewItemViewModel> CreateHandicaps(List<DerivedAttributeModel> handicapAttributes)
        {
            handicapAttributes.Add(new DerivedAttributeModel(DerivedAttributeType.Unknown));

            var result = new List<HandicapListViewItemViewModel>();
            foreach (var handicap in handicapAttributes)
            {
                string title = null;
                string imageSource = null;
                var isChecked = false;

                switch (handicap.Type)
                {
                    case DerivedAttributeType.Unknown:
                    {
                        title = "Ignorieren";
                        break;
                    }
                    case DerivedAttributeType.BehinderungKampf:
                    {
                        title = "Kampf";
                        imageSource = "Images/kampf.png";
                            break;
                    }
                    case DerivedAttributeType.BehinderungAbenteuer:
                    {
                        title = "Abenteuer / Reise";
                        imageSource = "Images/inventar.png";
                        isChecked = true;
                            break;
                    }
                    case DerivedAttributeType.BehinderungGesamt:
                    {
                        title = "Gesamt";
                            break;
                    }
                }

                handicap.PropertyChanged += (sender, args) =>
                {
                    if(args.PropertyName == nameof(CalculableBaseModel.FinalValue))
                        LoadoutValueChanged?.Invoke(this, EventArgs.Empty);
                };
                var vm = new HandicapListViewItemViewModel(handicap, isChecked, imageSource, title);
                vm.HandicapValueChanged += (sender, args) => LoadoutValueChanged?.Invoke(this, EventArgs.Empty);

                result.Add(vm);
            }

            return result;
        }
    }
}
