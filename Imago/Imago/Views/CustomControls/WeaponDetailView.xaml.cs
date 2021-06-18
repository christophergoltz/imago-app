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
    public partial class WeaponDetailView : ContentView
    {
        public WeaponDetailView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty WeaponDetailViewModelProperty = BindableProperty.Create(
            "WeaponDetailViewModel", // the name of the bindable property
            typeof(WeaponDetailViewModel), // the bindable property type
            typeof(WeaponDetailView));

        public WeaponDetailViewModel WeaponDetailViewModel
        {
            get => (WeaponDetailViewModel)GetValue(WeaponDetailViewModelProperty);
            set => SetValue(WeaponDetailViewModelProperty, value);
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