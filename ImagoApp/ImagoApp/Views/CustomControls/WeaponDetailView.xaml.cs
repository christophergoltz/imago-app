using System;
using System.Windows.Input;
using ImagoApp.Application.Models;
using ImagoApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImagoApp.Views.CustomControls
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
    }
}