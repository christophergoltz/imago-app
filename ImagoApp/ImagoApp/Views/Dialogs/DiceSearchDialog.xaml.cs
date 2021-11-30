using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImagoApp.ViewModels;
using ImagoApp.ViewModels.Dialog;
using ImagoApp.Views.CustomControls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImagoApp.Views.Dialogs
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DiceSearchDialog : ContentView
    {
        public DiceSearchDialog()
        {
            InitializeComponent();
        }

        private void SelectableItemsView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (e.CurrentSelection == null || !e.CurrentSelection.Any())
                return;
            
            var t = e.CurrentSelection;

            Device.BeginInvokeOnMainThread(() =>
            {
                ((CollectionView)sender).SelectedItem = null;
            });
           
            if (!t.Any())
            {
                DiceSearchDialogViewModel.SetSelectedItem(null);
                return;
            }

            var ee = t.First();

            if (ee is DiceSearchModel diceSearchModel)
                DiceSearchDialogViewModel.SetSelectedItem(diceSearchModel);
        }

        public static readonly BindableProperty DiceSearchDialogViewModelProperty = BindableProperty.Create(
            nameof(DiceSearchDialogViewModel), // the name of the bindable property
            typeof(DiceSearchDialogViewModel), // the bindable property type
            typeof(DiceSearchDialog));

        public DiceSearchDialogViewModel DiceSearchDialogViewModel
        {
            get => (DiceSearchDialogViewModel)GetValue(DiceSearchDialogViewModelProperty);
            set => SetValue(DiceSearchDialogViewModelProperty, value);
        }
    }
}