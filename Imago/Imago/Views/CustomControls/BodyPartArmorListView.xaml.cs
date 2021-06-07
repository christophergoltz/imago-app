using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Imago.Models;
using Imago.Models.Enum;
using Imago.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Imago.Views.CustomControls
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