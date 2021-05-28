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
    public partial class SkillGroupView : ContentView
    {
        public SkillGroupView()
        {
            InitializeComponent();
        }

        //public SkillGroup SkillGroup
        //{
        //    get => (SkillGroup) GetValue(SkillGroupProperty);
        //    set => SetValue(SkillGroupProperty, value);
        //}

        //public static readonly BindableProperty SkillGroupProperty =
        //    BindableProperty.Create(
        //        "SkillGroup",
        //        typeof(SkillGroup),
        //        typeof(SkillGroupView)
        //    );

        //public List<Skill> Skills
        //{
        //    get => (List<Skill>) GetValue(SkillProperty);
        //    set => SetValue(SkillProperty, value);
        //}

        //public static readonly BindableProperty SkillProperty =
        //    BindableProperty.Create(
        //        "Skills",
        //        typeof(List<Skill>),
        //        typeof(SkillGroupView)
        //    );


        public static readonly BindableProperty SkillsProperty = BindableProperty.Create(
            "Skills",        // the name of the bindable property
            typeof(List<Skill>),     // the bindable property type
            typeof(SkillGroupView));     

        public List<Skill> Skills
        {
            get => (List<Skill>)GetValue(SkillsProperty);
            set => SetValue(SkillsProperty, value);
        }

        public static readonly BindableProperty SkillGroupProperty = BindableProperty.Create(
            "SkillGroup",        // the name of the bindable property
            typeof(SkillGroup),     // the bindable property type
            typeof(SkillGroupView));

        public SkillGroup SkillGroup
        {
            get => (SkillGroup)GetValue(SkillGroupProperty);
            set => SetValue(SkillGroupProperty, value);
        }
    }
}