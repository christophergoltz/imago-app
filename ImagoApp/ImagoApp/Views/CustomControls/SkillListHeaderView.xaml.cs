using ImagoApp.Application.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImagoApp.Views.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SkillListHeaderView : ContentView
    {
        public SkillListHeaderView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty SkillGroupProperty = BindableProperty.Create(
            "SkillGroup",        // the name of the bindable property
            typeof(SkillGroupModel),     // the bindable property type
            typeof(SkillListHeaderView));

        public SkillGroupModel SkillGroup
        {
            get => (SkillGroupModel)GetValue(SkillGroupProperty);
            set => SetValue(SkillGroupProperty, value);
        }
    }
}