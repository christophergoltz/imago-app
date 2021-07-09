using ImagoApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImagoApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class InventoryPage : ContentPage
    {
        public InventoryPage(InventoryViewModel inventoryViewModel)
        {
            BindingContext = inventoryViewModel;
            InitializeComponent();
        }
    }
}