using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using Imago.Models;
using Imago.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Imago.Views.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WikiEntryPage : ContentPage
    {
        public WikiEntryPage()
        {
            InitializeComponent();
        }

        private void WebView_OnNavigated(object sender, WebNavigatedEventArgs e)
        {
            if (BindingContext is WikiEntryPageViewModel viewModel)
            {
                viewModel.WikiPageEntry.Url = e.Url;

                //no title given, try to create one
                string title;

                if (e.Url.Contains(WikiPageViewModel.WikiUrlPrefix))
                    title = e.Url.Replace(WikiPageViewModel.WikiUrlPrefix, "");
                else
                    title = e.Url.Split('/').Last();

                var newTitle = HttpUtility.UrlDecode(title);
                viewModel.WikiPageEntry.Title = newTitle;
                
                Debug.WriteLine("Updated WikiPageEntry.Url to " + e.Url + "; Title to " + newTitle);
            }
        }

        private void WebView_OnNavigating(object sender, WebNavigatingEventArgs e)
        {
            if (e.NavigationEvent != WebNavigationEvent.NewPage)
                return;

            if (BindingContext is WikiEntryPageViewModel vm)
            {
                var onlyPage = vm.WikiPageEntry;

                if (e.Url != onlyPage.Url && e.Url.StartsWith(onlyPage.Url))
                {
                    //bug: cancel due to uwp https://github.com/xamarin/Xamarin.Forms/issues/9005
                    e.Cancel = true;
                    DisplayAlert("Navigation abgebrochen",
                        $"Die Navigation zu{Environment.NewLine}\"{e.Url}\" wurde abgebrochen, da sonst die Anwendung abstürzten würde.{Environment.NewLine}{Environment.NewLine}Fehler: https://github.com/xamarin/Xamarin.Forms/issues/9005",
                        "OK");
                    return;
                }

                if (onlyPage.Url != WikiPageViewModel.WikiMainPageUrl && e.Url == WikiPageViewModel.WikiMainPageUrl)
                {
                    //user wants to navigate to wiki mainpage, dont create a new one, focus first tab
                    WikiPageViewModel.Instance.GoToStartWikiPage();
                    e.Cancel = true;
                    return;
                }

                if (e.Url != WikiPageViewModel.WikiMainPageUrl)
                {
                    Debug.WriteLine("Cancelling WikiNavigation, Opening new Page for: " + e.Url);
                    e.Cancel = true;

                    WikiPageViewModel.RequestedWikiPage = new WikiPageEntry(e.Url);
                    WikiPageViewModel.Instance.OpenWikiPage();
                }
            }
        }
    }
}