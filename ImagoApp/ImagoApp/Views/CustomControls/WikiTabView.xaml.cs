using System;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Windows.Input;
using ImagoApp.Application;
using ImagoApp.Application.Models;
using ImagoApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImagoApp.Views.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WikiTabView : ContentPage
    {
        public WikiTabView()
        {
            InitializeComponent();
        }

        private ICommand _closeTabWrapCommand;

        public ICommand CloseTabWrapCommand => _closeTabWrapCommand ?? (_closeTabWrapCommand = new Command(() =>
        {
            CloseTabCommand?.Execute(WikiTabModel);
        }, () =>
        {
            if (WikiTabModel == null)
                return false;

            var isClosable = WikiTabModel.Url != WikiConstants.WikiMainPageUrl;
            return isClosable;
        }));

        //open new page by url
        public static readonly BindableProperty OpenPageCommandProperty = BindableProperty.Create(
            nameof(OpenPageCommand),
            typeof(Command<string>),
            typeof(WikiTabView),
            null);

        public Command<string> OpenPageCommand
        {
            get { return (Command<string>)GetValue(OpenPageCommandProperty); }
            set { SetValue(OpenPageCommandProperty, value); }
        }

        //go back to wiki mainpage
        public static readonly BindableProperty GoBackToMainpageCommandProperty = BindableProperty.Create(
            nameof(GoBackToMainpageCommand),
            typeof(ICommand),
            typeof(WikiTabView),
            null);

        public ICommand GoBackToMainpageCommand
        {
            get { return (ICommand)GetValue(GoBackToMainpageCommandProperty); }
            set { SetValue(GoBackToMainpageCommandProperty, value); }
        }

        //close tab
        public static readonly BindableProperty CloseTabCommandProperty = BindableProperty.Create(
            nameof(CloseTabCommand),
            typeof(Command<WikiTabModel>),
            typeof(WikiTabView),
            null);

        public Command<WikiTabModel> CloseTabCommand
        {
            get { return (Command<WikiTabModel>)GetValue(CloseTabCommandProperty); }
            set { SetValue(CloseTabCommandProperty, value); }
        }

        public static readonly BindableProperty WikiTabModelProperty = BindableProperty.Create(
            nameof(WikiTabModel),
            typeof(WikiTabModel),
            typeof(WikiTabView),
            null, propertyChanged: (bindableObject, oldValue, newValue) =>
            {
                var wikiTabView = (WikiTabView) bindableObject;
                ((Command)wikiTabView.CloseTabWrapCommand).ChangeCanExecute();
            });

        public WikiTabModel WikiTabModel
        {
            get { return (WikiTabModel) GetValue(WikiTabModelProperty); }
            set { SetValue(WikiTabModelProperty, value); }
        }

        private void WebView_OnNavigated(object sender, WebNavigatedEventArgs e)
        {
            WikiTabModel.Url = e.Url;

            //no title given, try to create one
            string title;

            if (e.Url.Contains(WikiConstants.WikiUrlPrefix))
                title = e.Url.Replace(WikiConstants.WikiUrlPrefix, "");
            else
                title = e.Url.Split('/').Last();

            var newTitle = HttpUtility.UrlDecode(title);
            WikiTabModel.Title = newTitle;
        }

        private void WebView_OnNavigating(object sender, WebNavigatingEventArgs e)
        {
            if (e.NavigationEvent != WebNavigationEvent.NewPage)
                return;
            
            if (e.Url != WikiTabModel.Url && e.Url.StartsWith(WikiTabModel.Url))
            {
                //bug: cancel due to uwp https://github.com/xamarin/Xamarin.Forms/issues/9005
                e.Cancel = true;
                DisplayAlert("Navigation abgebrochen",
                    $"Die Navigation zu{Environment.NewLine}\"{e.Url}\" wurde abgebrochen, da sonst die Anwendung abstürzten würde.{Environment.NewLine}{Environment.NewLine}Fehler: https://github.com/xamarin/Xamarin.Forms/issues/9005",
                    "OK");
                return;
            }

            if (WikiTabModel.Url != WikiConstants.WikiMainPageUrl && e.Url == WikiConstants.WikiMainPageUrl)
            {
                //user wants to navigate to wiki mainpage, dont create a new one, focus first tab
                GoBackToMainpageCommand?.Execute(null);
                e.Cancel = true;
                return;
            }

            if (e.Url != WikiConstants.WikiMainPageUrl)
            {
                e.Cancel = true;

                OpenPageCommand?.Execute(e.Url);
            }
        }
    }
}