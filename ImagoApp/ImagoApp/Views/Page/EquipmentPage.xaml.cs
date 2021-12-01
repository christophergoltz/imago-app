using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImagoApp.ViewModels.Page;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImagoApp.Views.Page
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EquipmentPage : ContentPage
    {
        public EquipmentPageViewModel ViewModel { get; }

        public EquipmentPage(EquipmentPageViewModel equipmentPageViewModel)
        {
            BindingContext = ViewModel = equipmentPageViewModel;
            InitializeComponent();
        }
    }
}