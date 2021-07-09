using ImagoApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImagoApp.Views.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BodyPartArmorListView : ContentView
    {
        public BodyPartArmorListView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty BodyPartArmorListViewModelProperty = BindableProperty.Create(
            "BodyPartArmorListViewModel", // the name of the bindable property
            typeof(BodyPartArmorListViewModel), // the bindable property type
            typeof(BodyPartArmorListView));

        public BodyPartArmorListViewModel BodyPartArmorListViewModel
        {
            get => (BodyPartArmorListViewModel) GetValue(BodyPartArmorListViewModelProperty);
            set => SetValue(BodyPartArmorListViewModelProperty, value);
        }
    }
}