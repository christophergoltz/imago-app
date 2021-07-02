using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImagoApp.Views.CustomControls
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
            typeof(ViewModels.SkillGroupViewModel),     // the bindable property type
            typeof(SkillGroupView));

        public ViewModels.SkillGroupViewModel SkillGroupViewModel
        {
            get => (ViewModels.SkillGroupViewModel)GetValue(SkillGroupViewModelProperty);
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
        
        public ICommand SkillBaseTapCommand => new Command<Models.Base.DependentBase>(parameter =>
        {
            if (parameter is Models.SkillGroupModel group)
            {
                if (OpenSkillGroupCommand == null) 
                    return;
             
                if (OpenSkillGroupCommand.CanExecute(group))
                    OpenSkillGroupCommand.Execute(group);

                return;
            }
            
            if (parameter is Models.SkillModel skill)
            {
                if (OpenSkillCommand == null)
                    return;

                if (OpenSkillCommand.CanExecute((skill, SkillGroupViewModel.SkillGroup)))
                    OpenSkillCommand.Execute((skill, SkillGroupViewModel.SkillGroup));
                
            }
        });
    }
}