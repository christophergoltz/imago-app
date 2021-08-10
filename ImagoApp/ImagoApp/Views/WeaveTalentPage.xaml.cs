using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImagoApp.Application.Models;
using ImagoApp.ViewModels;
using ImagoApp.Views.CustomControls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImagoApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WeaveTalentPage : ContentPage
    {
        public WeaveTalentPageViewModel ViewModel { get; }
        public WeaveTalentPage(WeaveTalentPageViewModel weaveTalentPageViewModel)
        {
            BindingContext = ViewModel = weaveTalentPageViewModel;
            InitializeComponent();
        }
        
        private void SelectableItemsView_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (!e.CurrentSelection.Any())
                return;

            var selectedItem = (WeaveTalentModel)e.CurrentSelection.First();
            ViewModel.OpenWeaveTalentCommand?.Execute(selectedItem);
        }

        private void TapGestureRecognizer_OnTapped(object sender, EventArgs e)
        {
            WeaveTalentCollectionView.SelectedItem = null;
        }
    }
}