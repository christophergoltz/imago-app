using System;
using System.Linq;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImagoApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class StartPage : ContentPage
    {
        public StartPage()
        {
            InitializeComponent();
        }

        private void StartPage_OnAppearing(object sender, EventArgs e)
        {
            if (BindingContext is ViewModels.StartPageViewModel viewModel)
            {
                viewModel.WikiParseLog.CollectionChanged += (o, args) =>
                {
                    var last = viewModel.WikiParseLog.LastOrDefault();
                    if (last != null)
                        WikiParseFeedListView.ScrollTo(last, ScrollToPosition.Start, true);
                };
            }
        }
    }
}