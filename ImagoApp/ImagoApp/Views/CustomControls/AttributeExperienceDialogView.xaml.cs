using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;
using ImagoApp.Shared.Enums;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImagoApp.Views.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class AttributeExperienceDialogView : ContentView
    {
        public AttributeExperienceDialogView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty AttributeExperienceDialogViewModelProperty = BindableProperty.Create(
            "AttributeExperienceDialogViewModel", // the name of the bindable property
            typeof(ViewModels.AttributeExperienceDialogViewModel), // the bindable property type
            typeof(AttributeExperienceDialogView));

        public ViewModels.AttributeExperienceDialogViewModel AttributeExperienceDialogViewModel
        {
            get => (ViewModels.AttributeExperienceDialogViewModel) GetValue(AttributeExperienceDialogViewModelProperty);
            set => SetValue(AttributeExperienceDialogViewModelProperty, value);
        }

        public static readonly BindableProperty CloseCommandProperty = BindableProperty.Create(
            "CloseCommand",
            typeof(ICommand),
            typeof(AttributeExperienceDialogView),
            null);

        public ICommand CloseCommand
        {
            get { return (ICommand)GetValue(CloseCommandProperty); }
            set { SetValue(CloseCommandProperty, value); }
        }

        public static readonly BindableProperty SaveCommandProperty = BindableProperty.Create(
            "SaveCommand",
            typeof(ICommand),
            typeof(AttributeExperienceDialogView),
            null);

        public ICommand SaveCommand
        {
            get { return (ICommand)GetValue(SaveCommandProperty); }
            set { SetValue(SaveCommandProperty, value); }
        }


        private void OnDragStarting(object sender, DragStartingEventArgs e)
        {
            if (sender is Element element)
            {
                if (element.BindingContext is ViewModels.OpenAttributeExperienceViewModel viewModel)
                {
                    ResetDropHighlight(true);

                    e.Data.Properties.Add(nameof(ViewModels.OpenAttributeExperienceViewModel), viewModel);
                    
                    var greenColor = (Color)Xamarin.Forms.Application.Current.Resources["HellGruenesUmbra2"];

                    foreach (var attribute in viewModel.PossibleTargets)
                    {
                        switch (attribute.Type)
                        {
                            case AttributeType.Unknown:
                                break;
                            case AttributeType.Staerke:
                                Staerke.BackgroundColor = greenColor;
                                ((DropGestureRecognizer)Staerke.GestureRecognizers.First(_ => _.GetType() == typeof(DropGestureRecognizer))).AllowDrop = true;
                                break;
                            case AttributeType.Geschicklichkeit:
                                Geschicklichkeit.BackgroundColor = greenColor;
                                ((DropGestureRecognizer)Geschicklichkeit.GestureRecognizers.First(_ => _.GetType() == typeof(DropGestureRecognizer))).AllowDrop = true;
                                break;
                            case AttributeType.Konstitution:
                                Konstitution.BackgroundColor = greenColor;
                                ((DropGestureRecognizer)Konstitution.GestureRecognizers.First(_ => _.GetType() == typeof(DropGestureRecognizer))).AllowDrop = true;
                                break;
                            case AttributeType.Intelligenz:
                                Intelligenz.BackgroundColor = greenColor;
                                ((DropGestureRecognizer)Intelligenz.GestureRecognizers.First(_ => _.GetType() == typeof(DropGestureRecognizer))).AllowDrop = true;
                                break;
                            case AttributeType.Willenskraft:
                                Willenskraft.BackgroundColor = greenColor;
                                ((DropGestureRecognizer)Willenskraft.GestureRecognizers.First(_ => _.GetType() == typeof(DropGestureRecognizer))).AllowDrop = true;
                                break;
                            case AttributeType.Charisma:
                                Charisma.BackgroundColor = greenColor;
                                ((DropGestureRecognizer)Charisma.GestureRecognizers.First(_ => _.GetType() == typeof(DropGestureRecognizer))).AllowDrop = true;
                                break;
                            case AttributeType.Wahrnehmung:
                                Wahrnehmung.BackgroundColor = greenColor;
                                ((DropGestureRecognizer)Wahrnehmung.GestureRecognizers.First(_ => _.GetType() == typeof(DropGestureRecognizer))).AllowDrop = true;
                                break;
                        }
                    }
                }
            }
        }

        private void ResetDropHighlight(bool gestures)
        {
            var color = (Color)Xamarin.Forms.Application.Current.Resources["AntiUmbra3"];

            Charisma.BackgroundColor = color;
            Staerke.BackgroundColor = color;
            Wahrnehmung.BackgroundColor = color;
            Willenskraft.BackgroundColor = color;
            Konstitution.BackgroundColor = color;
            Intelligenz.BackgroundColor = color;
            Geschicklichkeit.BackgroundColor = color;

            if (gestures)
            {
                ((DropGestureRecognizer)Charisma.GestureRecognizers.First(_ => _.GetType() == typeof(DropGestureRecognizer))).AllowDrop = false;
                ((DropGestureRecognizer)Wahrnehmung.GestureRecognizers.First(_ => _.GetType() == typeof(DropGestureRecognizer))).AllowDrop = false;
                ((DropGestureRecognizer)Staerke.GestureRecognizers.First(_ => _.GetType() == typeof(DropGestureRecognizer))).AllowDrop = false;
                ((DropGestureRecognizer)Willenskraft.GestureRecognizers.First(_ => _.GetType() == typeof(DropGestureRecognizer))).AllowDrop = false;
                ((DropGestureRecognizer)Konstitution.GestureRecognizers.First(_ => _.GetType() == typeof(DropGestureRecognizer))).AllowDrop = false;
                ((DropGestureRecognizer)Intelligenz.GestureRecognizers.First(_ => _.GetType() == typeof(DropGestureRecognizer))).AllowDrop = false;
                ((DropGestureRecognizer)Geschicklichkeit.GestureRecognizers.First(_ => _.GetType() == typeof(DropGestureRecognizer))).AllowDrop = false;
            }
        }

        private void RemoveFromAll(ViewModels.OpenAttributeExperienceViewModel viewModel)
        {
            AttributeExperienceDialogViewModel.OpenAttributeExperience.Remove(viewModel);
            AttributeExperienceDialogViewModel.Charisma.Remove(viewModel);
            AttributeExperienceDialogViewModel.Staerke.Remove(viewModel);
        }

        private void DropGestureRecognizer_OnDrop(object sender, DropEventArgs e)
        {
            if (sender is Element element)
            {
                var collection = GetItemSource(element.Parent);
                var data =
                    e.Data.Properties[nameof(ViewModels.OpenAttributeExperienceViewModel)] as ViewModels.OpenAttributeExperienceViewModel;

                RemoveFromAll(data);
                collection.Add(data);
                ResetDropHighlight(false);
            }
        }

        private ObservableCollection<ViewModels.OpenAttributeExperienceViewModel> GetItemSource(Element element)
        {
            if (element == Staerke)
                return AttributeExperienceDialogViewModel.Staerke;
            if (element == Charisma)
                return AttributeExperienceDialogViewModel.Charisma;
            if (element == Intelligenz)
                return AttributeExperienceDialogViewModel.Intelligenz;
            if (element == Geschicklichkeit)
                return AttributeExperienceDialogViewModel.Geschicklichkeit;
            if (element == Konstitution)
                return AttributeExperienceDialogViewModel.Konstitution;
            if (element == Wahrnehmung)
                return AttributeExperienceDialogViewModel.Wahrnehmung;
            if (element == Willenskraft)
                return AttributeExperienceDialogViewModel.Willenskraft;
            
            throw new InvalidOperationException("Unkown ItemSource of <OpenAttributeExperienceViewModel> for " + element);
        }

        private void DragGestureRecognizer_OnDropCompleted(object sender, DropCompletedEventArgs e)
        {
            ResetDropHighlight(true);
        }
    }
}