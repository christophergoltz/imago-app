using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ImagoApp.Application;
using ImagoApp.Application.Constants;
using ImagoApp.Application.Models;
using ImagoApp.Shared.Enums;

namespace ImagoApp.ViewModels
{
    public class LoadoutViewModel : BindableBase
    {
        private readonly List<DerivedAttributeModel> _handicapAttributes;
        public event EventHandler LoadoutValueChanged;

        public LoadoutViewModel(List<DerivedAttributeModel> handicapAttributes)
        {
            _handicapAttributes = handicapAttributes;

            InitializeHandicaps();
        }

        private List<HandicapListViewItemViewModel> _handicaps;
        public List<HandicapListViewItemViewModel> Handicaps
        {
            get => _handicaps;
            set {
                SetProperty(ref _handicaps, value);
            }
        }

        public int GetLoadoutValue()
        {
            return Handicaps.FirstOrDefault(model => model.IsChecked)?.HandiCapValue ?? 0;
        }

        private static readonly List<(DerivedAttributeType Type, string Text, string IconSource)> HandicapDefinition =
            new List<(DerivedAttributeType Type, string Text, string IconSource)>()
            {
                (DerivedAttributeType.BehinderungKampf, "Kampf", "Images/kampf.png"),
                (DerivedAttributeType.BehinderungAbenteuer, "Abenteuer / Reise", "Images/inventar.png"),
                (DerivedAttributeType.BehinderungGesamt, "Gesamt", null),
                (DerivedAttributeType.Unknown, "Ignorieren", null)
            };

        private void InitializeHandicaps()
        {
            List<HandicapListViewItemViewModel> handicaps = new List<HandicapListViewItemViewModel>();

            //only add handicap for attributessource with Geschicklichkeit
            foreach (var tuple in HandicapDefinition)
            {
                var handicapValue = tuple.Type == DerivedAttributeType.Unknown
                    ? (int?)null
                    : _handicapAttributes.First(attribute => attribute.Type == tuple.Type).FinalValue.GetRoundedValue();

                //todo converter
                var vm = new HandicapListViewItemViewModel(tuple.Type, false, handicapValue, tuple.IconSource,
                    tuple.Text);
                vm.HandicapValueChanged += (sender, args) => LoadoutValueChanged?.Invoke(this, EventArgs.Empty);

                if (vm.Type == DerivedAttributeType.BehinderungAbenteuer)
                    vm.IsChecked = true;

                handicaps.Add(vm);
            }

            Handicaps = handicaps;
        }
    }
}
