using ImagoApp.Application.Models;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImagoApp.Views.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class CharacterDetailView : ContentView
    {
        public CharacterDetailView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty CharacterProperty = BindableProperty.Create(
            "Character", // the name of the bindable property
            typeof(CharacterModel), // the bindable property type
            typeof(CharacterDetailView));

        public CharacterModel CharacterModel
        {
            get => (CharacterModel)GetValue(CharacterProperty);
            set => SetValue(CharacterProperty, value);
        }

    }
}