using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Imago.Models;
using Imago.Models.Base;
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
       
        public static readonly BindableProperty SkillGroupProperty = BindableProperty.Create(
            "SkillGroup",        // the name of the bindable property
            typeof(SkillGroup),     // the bindable property type
            typeof(SkillGroupView));

        public SkillGroup SkillGroup
        {
            get => (SkillGroup)GetValue(SkillGroupProperty);
            set => SetValue(SkillGroupProperty, value);
        }
        
        public static readonly BindableProperty OpenSkillCommandProperty = BindableProperty.Create(
            "OpenSkillCommand", 
            typeof(ICommand), 
            typeof(SkillGroupView), 
            null);

        public ICommand OpenSkillCommand
        {
            get { return (ICommand)GetValue(OpenSkillCommandProperty); }
            set { SetValue(OpenSkillCommandProperty, value); }
        }

        public static readonly BindableProperty OpenSkillGroupCommandProperty = BindableProperty.Create(
            "OpenSkillGroupCommand",
            typeof(ICommand),
            typeof(SkillGroupView),
            null);

        public ICommand OpenSkillGroupCommand
        {
            get { return (ICommand)GetValue(OpenSkillGroupCommandProperty); }
            set { SetValue(OpenSkillGroupCommandProperty, value); }
        }
        
        public ICommand SkillBaseTapCommand => new Command<IncreasableBase>(parameter =>
        {
            if (parameter is SkillGroup group)
            {
                if (OpenSkillGroupCommand == null) 
                    return;
             
                if (OpenSkillGroupCommand.CanExecute(group))
                    OpenSkillGroupCommand.Execute(group);

                return;
            }
            
            if (parameter is Skill skill)
            {
                if (OpenSkillCommand == null)
                    return;

                if (OpenSkillCommand.CanExecute((skill, SkillGroup)))
                    OpenSkillCommand.Execute((skill, SkillGroup));
                
            }
        });
    }
}