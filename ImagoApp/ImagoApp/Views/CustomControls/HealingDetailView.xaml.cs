using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImagoApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImagoApp.Views.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HealingDetailView : ContentView
    {
        public HealingDetailView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty HealingDetailViewModelProperty = BindableProperty.Create(
            nameof(HealingDetailViewModel),// the name of the bindable property
            typeof(HealingDetailViewModel), // the bindable property type
            typeof(HealingDetailView));

        public HealingDetailViewModel HealingDetailViewModel
        {
            get => (HealingDetailViewModel)GetValue(HealingDetailViewModelProperty);
            set => SetValue(HealingDetailViewModelProperty, value);
        }
    }
}