using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Imago.Models;
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

        public static readonly BindableProperty BodyPartProperty = BindableProperty.Create(
            "BodyPart", // the name of the bindable property
            typeof(BodyPart), // the bindable property type
            typeof(BodyPartArmorListView));

        public BodyPart BodyPart
        {
            get => (BodyPart)GetValue(BodyPartProperty);
            set => SetValue(BodyPartProperty, value);
        }
    }
}