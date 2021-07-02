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
            typeof(Models.SkillGroupModel),     // the bindable property type
            typeof(SkillListHeaderView));

        public Models.SkillGroupModel SkillGroup
        {
            get => (Models.SkillGroupModel)GetValue(SkillGroupProperty);
            set => SetValue(SkillGroupProperty, value);
        }
    }
}