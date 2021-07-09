using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImagoApp.Views.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SkillGroupDetailView : ContentView
    {
        public SkillGroupDetailView()
        {
            InitializeComponent();
            BindingContext = this;
        }

        public static readonly BindableProperty SkillGroupDetailViewModelProperty = BindableProperty.Create(
            "SkillGroupDetailViewModel", // the name of the bindable property
            typeof(ViewModels.SkillGroupDetailViewModel), // the bindable property type
            typeof(SkillGroupDetailView));

        public ViewModels.SkillGroupDetailViewModel SkillGroupDetailViewModel
        {
            get => (ViewModels.SkillGroupDetailViewModel)GetValue(SkillGroupDetailViewModelProperty);
            set => SetValue(SkillGroupDetailViewModelProperty, value);
        }

        private void WebView_OnNavigating(object sender, WebNavigatingEventArgs e)
        {
            if (e.Url != SkillGroupDetailViewModel.QuickWikiView.BaseUrl)
                e.Cancel = true;
        }
    }
}