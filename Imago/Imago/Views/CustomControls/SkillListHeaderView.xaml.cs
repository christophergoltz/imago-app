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
    public partial class SkillListHeaderView : ContentView
    {
        public SkillListHeaderView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty SkillGroupProperty = BindableProperty.Create(
            "SkillGroup",        // the name of the bindable property
            typeof(SkillGroup),     // the bindable property type
            typeof(SkillListHeaderView));

        public SkillGroup SkillGroup
        {
            get => (SkillGroup)GetValue(SkillGroupProperty);
            set => SetValue(SkillGroupProperty, value);
        }
    }
}