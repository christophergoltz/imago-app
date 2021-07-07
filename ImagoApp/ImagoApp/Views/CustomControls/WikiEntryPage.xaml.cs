using System;
using System.Diagnostics;
using System.Linq;
using System.Web;
using ImagoApp.Application.Models;
using ImagoApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImagoApp.Views.CustomControls
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

                if (e.Url.Contains(Util.WikiConstants.WikiUrlPrefix))
                    title = e.Url.Replace(Util.WikiConstants.WikiUrlPrefix, "");
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

                if (onlyPage.Url != Util.WikiConstants.WikiMainPageUrl && e.Url == Util.WikiConstants.WikiMainPageUrl)
                {
                    //user wants to navigate to wiki mainpage, dont create a new one, focus first tab
                    WikiPageViewModel.Instance.GoToStartWikiPage();
                    e.Cancel = true;
                    return;
                }

                if (e.Url != Util.WikiConstants.WikiMainPageUrl)
                {
                    Debug.WriteLine("Cancelling WikiNavigation, Opening new Page for: " + e.Url);
                    e.Cancel = true;
                    
                    vm.RaiseOpenWikiPageRequested(e.Url);
                }
            }
        }
    }
}