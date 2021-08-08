using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ImagoApp.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImagoApp.Views.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WeaveTalentDetailView : ContentView
    {
        public WeaveTalentDetailView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty WeaveTalentDetailViewModelProperty = BindableProperty.Create(
            "WeaveTalentDetailViewModel", // the name of the bindable property
            typeof(WeaveTalentDetailViewModel), // the bindable property type
            typeof(WeaveTalentDetailView));

        public WeaveTalentDetailViewModel WeaveTalentDetailViewModel
        {
            get => (WeaveTalentDetailViewModel)GetValue(WeaveTalentDetailViewModelProperty);
            set => SetValue(WeaveTalentDetailViewModelProperty, value);
        }

        public static readonly BindableProperty CloseCommandProperty =
            BindableProperty.Create(nameof(CloseCommand), typeof(ICommand), typeof(SkillGroupView), null);

        public ICommand CloseCommand
        {
            get { return (ICommand)GetValue(CloseCommandProperty); }
            set { SetValue(CloseCommandProperty, value); }
        }
    }
}