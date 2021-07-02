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
            BindingContext = this;
        }
        
        public static readonly BindableProperty SkillDetailViewModelProperty = BindableProperty.Create(
            "SkillDetailViewModel", // the name of the bindable property
            typeof(ViewModels.SkillDetailViewModel), // the bindable property type
            typeof(SkillDetailView));

        public ViewModels.SkillDetailViewModel SkillDetailViewModel
        {
            get => (ViewModels.SkillDetailViewModel)GetValue(SkillDetailViewModelProperty);
            set => SetValue(SkillDetailViewModelProperty, value);
        }

        private void WebView_OnNavigating(object sender, WebNavigatingEventArgs e)
        {
            if (e.Url != SkillDetailViewModel.QuickWikiView.BaseUrl)
                e.Cancel = true;
        }
    }
}