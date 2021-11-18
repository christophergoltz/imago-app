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
    public partial class TreatmentDetailView : ContentView
    {
        public TreatmentDetailView()
        {
            InitializeComponent();
        }
        
        public static readonly BindableProperty TreatmentDetailViewModelProperty = BindableProperty.Create(
            nameof(TreatmentDetailViewModel),// the name of the bindable property
            typeof(TreatmentDetailViewModel), // the bindable property type
            typeof(TreatmentDetailView));

        public TreatmentDetailViewModel TreatmentDetailViewModel
        {
            get => (TreatmentDetailViewModel)GetValue(TreatmentDetailViewModelProperty);
            set => SetValue(TreatmentDetailViewModelProperty, value);
        }
    }
}