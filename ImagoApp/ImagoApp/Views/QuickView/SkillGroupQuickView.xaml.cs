using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using ImagoApp.ViewModels;
using ImagoApp.Views.CustomControls;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImagoApp.Views.QuickView
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class SkillGroupQuickView : ContentView
    {
        public SkillGroupQuickView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty SkillGroupViewModelProperty = BindableProperty.Create(
            nameof(SkillGroupViewModel), // the name of the bindable property
            typeof(SkillGroupViewModel), // the bindable property type
            typeof(SkillGroupQuickView));

        public SkillGroupViewModel SkillGroupViewModel
        {
            get => (SkillGroupViewModel)GetValue(SkillGroupViewModelProperty);
            set => SetValue(SkillGroupViewModelProperty, value);
        }

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(
            nameof(Command), // the name of the bindable property
            typeof(ICommand), // the bindable property type
            typeof(SkillGroupQuickView));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly BindableProperty TabIndexProperty = BindableProperty.Create(
            nameof(TabIndex), // the name of the bindable property
            typeof(int), // the bindable property type
            typeof(SkillGroupQuickView));

        public int TabIndex
        {
            get => (int)GetValue(TabIndexProperty);
            set => SetValue(TabIndexProperty, value);
        }
    }
}