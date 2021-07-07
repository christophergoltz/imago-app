using ImagoApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImagoApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SkillPage : ContentPage
    {
        public SkillPage(SkillPageViewModel skillPageViewModel)
        {
            BindingContext = skillPageViewModel;
            InitializeComponent();
        }
    }
}