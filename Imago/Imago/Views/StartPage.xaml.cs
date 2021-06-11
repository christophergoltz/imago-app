using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Configuration.Annotations;
using Imago.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Imago.Views
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
            if (BindingContext is StartPageViewModel viewModel)
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