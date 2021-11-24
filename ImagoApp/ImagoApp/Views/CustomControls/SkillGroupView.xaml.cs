using System.Linq;
using System.Windows.Input;
using ImagoApp.Application.Models;
using ImagoApp.Application.Models.Base;
using ImagoApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImagoApp.Views.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SkillGroupView : ContentView
    {
        public SkillGroupView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty SkillGroupViewModelProperty = BindableProperty.Create(
            "SkillGroupViewModel",        // the name of the bindable property
            typeof(SkillGroupViewModel),     // the bindable property type
            typeof(SkillGroupView));

        public SkillGroupViewModel SkillGroupViewModel
        {
            get => (SkillGroupViewModel)GetValue(SkillGroupViewModelProperty);
            set => SetValue(SkillGroupViewModelProperty, value);
        }
        
        private void WebView_OnNavigating(object sender, WebNavigatingEventArgs e)
        {
            if (e.Url != SkillGroupViewModel.SkillWikiSource.BaseUrl)
                e.Cancel = true;
        }
    }
}