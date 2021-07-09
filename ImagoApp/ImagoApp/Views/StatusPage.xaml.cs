using ImagoApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImagoApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StatusPage : ContentPage
    {
        public StatusPage(StatusPageViewModel statusPageViewModel)
        {
            BindingContext = statusPageViewModel;
            InitializeComponent();
        }
    }
}