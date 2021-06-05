using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Imago.Models;
using Imago.Models.Enum;
using Imago.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Imago.Views.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BodyPartArmorListView : ContentView
    {
        public ICommand ChangeArmorCommand { get; set; }

        public BodyPartArmorListView()
        {
            InitializeComponent();

            ChangeArmorCommand = new Command<ArmorType>(async armorType =>
            {
                var availableArmor = new List<string>();
                foreach (var armor in BodyPartArmorListViewModel.ItemRepository.GetAllArmorParts(
                    BodyPartArmorListViewModel.BodyPart.Type))
                {
                    //exclude wearing items
                    if (!BodyPartArmorListViewModel.Armor.Select(armor1 => armor1.Type).Contains(armor.Type))
                    {
                        availableArmor.Add(armor.Type.ToString());
                    }
                }

                var result = await Shell.Current.DisplayActionSheet($"\"{armorType}\" ändern in", "Abbrechen",
                    "Rüstung enfernen", availableArmor.ToArray());

                if (result == null || result.Equals("Abbrechen"))
                    return;

                //remove old armor
                BodyPartArmorListViewModel.Armor.Remove(
                    BodyPartArmorListViewModel.Armor.First(armor => armor.Type == armorType));
                BodyPartArmorListViewModel.BodyPart.Armor.Remove(armorType);

                if (result.Equals("Rüstung enfernen"))
                {
                    //recalulate load
                    BodyPartArmorListViewModel.CharacterService.RecalculateHandicapAttributes(BodyPartArmorListViewModel.Character);
                    return;
                }

                //add selected armor
                var newArmor = (ArmorType) Enum.Parse(typeof(ArmorType), result);
                BodyPartArmorListViewModel.Armor.Add(
                    BodyPartArmorListViewModel.ItemRepository.GetArmorPart(newArmor,
                        BodyPartArmorListViewModel.BodyPart.Type));
                BodyPartArmorListViewModel.BodyPart.Armor.Add(newArmor);

                //recalulate load
                BodyPartArmorListViewModel.CharacterService.RecalculateHandicapAttributes(BodyPartArmorListViewModel.Character);
            });
        }

        public static readonly BindableProperty BodyPartArmorListViewModelProperty = BindableProperty.Create(
            "BodyPartArmorListViewModel", // the name of the bindable property
            typeof(BodyPartArmorListViewModel), // the bindable property type
            typeof(BodyPartArmorListView));

        public BodyPartArmorListViewModel BodyPartArmorListViewModel
        {
            get => (BodyPartArmorListViewModel) GetValue(BodyPartArmorListViewModelProperty);
            set => SetValue(BodyPartArmorListViewModelProperty, value);
        }
    }
}