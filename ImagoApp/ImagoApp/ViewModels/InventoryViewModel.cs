using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ImagoApp.Application;
using ImagoApp.Application.Models;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class InventoryViewModel
    {
        public event EventHandler<string> OpenWikiPageRequested;
        public CharacterViewModel CharacterViewModel { get; }

        private ICommand _removeItemCommand;

        public ICommand RemoveItemCommand => _removeItemCommand ?? (_removeItemCommand =
            new Command<EquippableItemViewModel>(item =>
            {
                try
                {
                    EquippableItemViewModels.Remove(item);
                    CharacterViewModel.CharacterModel.EquippedItems.Remove(item.EquipableItemModel);
                    CharacterViewModel.RecalculateHandicapAttributes();
                }
                catch (Exception e)
                {
                    App.ErrorManager.TrackException(e, CharacterViewModel.CharacterModel.Name);
                }
            }));

        private ICommand _openDailyGoodsWikiCommand;

        public ICommand OpenDailyGoodsWikiCommand => _openDailyGoodsWikiCommand ?? (_openDailyGoodsWikiCommand = new Command(() =>
        {
            try
            {
                var url = WikiConstants.DailyGoodsUrl;
                OpenWikiPageRequested?.Invoke(this, url);
            }
            catch (Exception exception)
            {
                App.ErrorManager.TrackException(exception, CharacterViewModel.CharacterModel.Name);
            }
        }));

        private ICommand _openAmmunitionCommand;

        public ICommand OpenAmmunitionCommand => _openAmmunitionCommand ?? (_openAmmunitionCommand = new Command(() =>
        {
            try
            {
                var url = WikiConstants.AmmunitionUrl;
                OpenWikiPageRequested?.Invoke(this, url);
            }
            catch (Exception exception)
            {
                App.ErrorManager.TrackException(exception, CharacterViewModel.CharacterModel.Name);
            }
        }));

        private ICommand _addItemCommand;

        public ICommand AddItemCommand => _addItemCommand ?? (_addItemCommand = new Command(() =>
        {
            try
            {
                var equipableItem = new EquipableItemModel(string.Empty, 0, false, false);
                CharacterViewModel.CharacterModel.EquippedItems.Add(equipableItem);
                EquippableItemViewModels.Add(new EquippableItemViewModel(equipableItem, CharacterViewModel));
            }
            catch (Exception e)
            {
                App.ErrorManager.TrackException(e, CharacterViewModel.CharacterModel.Name);
            }
        }));
        
        public ObservableCollection<EquippableItemViewModel> EquippableItemViewModels { get; set; }

        public InventoryViewModel(CharacterViewModel characterViewModel)
        {
            CharacterViewModel = characterViewModel;
            EquippableItemViewModels = new ObservableCollection<EquippableItemViewModel>(
                characterViewModel.CharacterModel.EquippedItems.Select(item => new EquippableItemViewModel(item, characterViewModel)));
        }
    }
}
