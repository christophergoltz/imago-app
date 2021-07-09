using ImagoApp.Application.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImagoApp.Views.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class QuickSkillGroupHeaderView : ContentView
    {
        public QuickSkillGroupHeaderView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty SkillGroupProperty = BindableProperty.Create(
            "SkillGroup",        // the name of the bindable property
            typeof(SkillGroupModel),     // the bindable property type
            typeof(QuickSkillGroupHeaderView));

        public SkillGroupModel SkillGroup
        {
            get => (SkillGroupModel)GetValue(SkillGroupProperty);
            set => SetValue(SkillGroupProperty, value);
        }
    }
}