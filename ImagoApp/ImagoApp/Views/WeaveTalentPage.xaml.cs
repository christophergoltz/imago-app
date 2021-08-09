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

        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)
                return;

            var selectedItem = (WeaveTalentModel)e.SelectedItem;
            
            //reset selection
            ((ListView)sender).SelectedItem = null;

            //reset listview selection
            var listView = (ListView)sender;
            listView.SelectedItem = null;

            ViewModel.OpenWeaveTalentCommand?.Execute(selectedItem);
        }
    }
}