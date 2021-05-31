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
        
        // BindableProperty implementation
        public static readonly BindableProperty CommandProperty =
            BindableProperty.Create(nameof(Command), typeof(ICommand), typeof(SkillGroupView), null);

        public ICommand Command
        {
            get { return (ICommand)GetValue(CommandProperty); }
            set { SetValue(CommandProperty, value); }
        }

        // Helper method for invoking commands safely
        public static void Execute(ICommand command, (SkillGroup SkillGroup, UpgradeableSkillBase SelectedUpgradeableSkill) parameter)
        {
            if (command == null) return;
            if (command.CanExecute(parameter))
            {
                command.Execute(parameter);
            }
        }
        
        public ICommand SkillDoubleTapCommand => new Command<UpgradeableSkillBase>(parameter =>
        {
            if(parameter is SkillGroup group)
                Execute(Command, (group, group));

            if (parameter is Skill skill)
                Execute(Command, (SkillGroup, skill));
        });
    }
}