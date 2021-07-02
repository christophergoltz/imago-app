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
    public partial class SkillGroupView : ContentView
    {
        public SkillGroupView()
        {
            InitializeComponent();
        }
       
        public static readonly BindableProperty SkillGroupViewModelProperty = BindableProperty.Create(
            "SkillGroupViewModel",        // the name of the bindable property
            typeof(SkillGroupViewModel),     // the bindable property type
            typeof(SkillGroupView));

        public SkillGroupViewModel SkillGroupViewModel
        {
            get => (SkillGroupViewModel)GetValue(SkillGroupViewModelProperty);
            set => SetValue(SkillGroupViewModelProperty, value);
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
        
        public ICommand SkillBaseTapCommand => new Command<DependentBase>(parameter =>
        {
            if (parameter is SkillGroupModel group)
            {
                if (OpenSkillGroupCommand == null) 
                    return;
             
                if (OpenSkillGroupCommand.CanExecute(group))
                    OpenSkillGroupCommand.Execute(group);

                return;
            }
            
            if (parameter is SkillModel skill)
            {
                if (OpenSkillCommand == null)
                    return;

                if (OpenSkillCommand.CanExecute((skill, SkillGroupViewModel.SkillGroup)))
                    OpenSkillCommand.Execute((skill, SkillGroupViewModel.SkillGroup));
                
            }
        });
    }
}