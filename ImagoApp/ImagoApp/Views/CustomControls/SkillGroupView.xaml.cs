using System.Windows.Input;
using ImagoApp.Application.Models;
using ImagoApp.Application.Models.Base;
using ImagoApp.ViewModels;
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

        public static readonly BindableProperty IconSourceProperty = BindableProperty.Create(
            "IconSource",        // the name of the bindable property
            typeof(string),     // the bindable property type
            typeof(SkillGroupView));

        public string IconSource
        {
            get => (string)GetValue(IconSourceProperty);
            set => SetValue(IconSourceProperty, value);
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
        
        public ICommand SkillBaseTapCommand => new Command<DependentBaseModel>(parameter =>
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
        
        private void ListView_OnItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null )
                return;
            var selectedSkill = (SkillViewModel)e.SelectedItem;

            //reset listview selection
            var listView = (ListView)sender;
            listView.SelectedItem = null;

            SkillBaseTapCommand?.Execute(selectedSkill.Skill);
        }
    }
}