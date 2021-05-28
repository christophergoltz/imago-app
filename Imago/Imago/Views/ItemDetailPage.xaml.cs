using Imago.ViewModels;
using System.ComponentModel;
using Xamarin.Forms;

namespace Imago.Views
{
    public partial class ItemDetailPage : ContentPage
    {
        public ItemDetailPage()
        {
            InitializeComponent();
            BindingContext = new ItemDetailViewModel();
        }
    }
}