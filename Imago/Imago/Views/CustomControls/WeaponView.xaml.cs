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
    public partial class WeaponView : ContentView
    {
        public WeaponView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty WeaponProperty = BindableProperty.Create(
            "Weapon", // the name of the bindable property
            typeof(Weapon), // the bindable property type
            typeof(WeaponView));

        public Weapon Weapon
        {
            get => (Weapon)GetValue(WeaponProperty);
            set => SetValue(WeaponProperty, value);
        }
    }
}