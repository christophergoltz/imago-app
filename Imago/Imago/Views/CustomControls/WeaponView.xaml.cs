using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Imago.Models;
using Imago.Models.Base;
using Imago.ViewModels;
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

        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(SkillGroupView), null);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }
        
        public ICommand RemoveWeaponCommand => new Command<Weapon>(weapon =>
        {
            if (Command.CanExecute(weapon))
            {
                Command.Execute(weapon);
            }
        });
    }
}