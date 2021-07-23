using ImagoApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImagoApp.Views.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SkillDetailView : ContentView
    {
        public SkillDetailView()
        {
            InitializeComponent();
        }
        
        public static readonly BindableProperty SkillDetailViewModelProperty = BindableProperty.Create(
            "SkillDetailViewModel", // the name of the bindable property
            typeof(SkillDetailViewModel), // the bindable property type
            typeof(SkillDetailView));

        public SkillDetailViewModel SkillDetailViewModel
        {
            get => (SkillDetailViewModel)GetValue(SkillDetailViewModelProperty);
            set => SetValue(SkillDetailViewModelProperty, value);
        }

        private void WebView_OnNavigating(object sender, WebNavigatingEventArgs e)
        {
            if (e.Url != SkillDetailViewModel.QuickWikiView.BaseUrl)
                e.Cancel = true;
        }
    }
}