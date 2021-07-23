using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Input;
using ImagoApp.Application.Constants;
using ImagoApp.Application.Models;
using ImagoApp.Shared.Enums;
using ImagoApp.ViewModels;
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
            ResetDropHighlight();
        }

        public static readonly BindableProperty CharacterViewModelProperty = BindableProperty.Create(
            "CharacterViewModel", // the name of the bindable property
            typeof(CharacterViewModel), // the bindable property type
            typeof(AttributeExperienceDialogView));

        public CharacterViewModel CharacterViewModel
        {
            get => (CharacterViewModel) GetValue(CharacterViewModelProperty);
            set => SetValue(CharacterViewModelProperty, value);
        }

        public static readonly BindableProperty CloseCommandProperty = BindableProperty.Create(
            "CloseCommand",
            typeof(ICommand),
            typeof(AttributeExperienceDialogView),
            null);

        public ICommand CloseCommand
        {
            get { return (ICommand) GetValue(CloseCommandProperty); }
            set { SetValue(CloseCommandProperty, value); }
        }


        private List<AttributeModel> GetDropTargets(SkillGroupModelType source)
        {
            var affectedAttributeTypes = RuleConstants.GetSkillGroupSources(source).Distinct().ToList();
            var affectedAttributes = CharacterViewModel.CharacterModel.Attributes
                .Where(attribute => affectedAttributeTypes.Contains(attribute.Type))
                .ToList();

            return affectedAttributes;
        }

        private void OnDragStarting(object sender, DragStartingEventArgs e)
        {
            if (sender is Element element)
            {
                if (element.BindingContext is SkillGroupModelType skillGroupModelType)
                {
                    ResetDropHighlight();
                    e.Data.Properties.Add(nameof(SkillGroupModelType), skillGroupModelType);

                    foreach (var attribute in GetDropTargets(skillGroupModelType))
                    {
                        switch (attribute.Type)
                        {
                            case AttributeType.Staerke:
                                SetDropGestureRecognizerAllow(Staerke, true);
                                break;
                            case AttributeType.Geschicklichkeit:
                                SetDropGestureRecognizerAllow(Geschicklichkeit, true);
                                break;
                            case AttributeType.Konstitution:
                                SetDropGestureRecognizerAllow(Konstitution, true);
                                break;
                            case AttributeType.Intelligenz:
                                SetDropGestureRecognizerAllow(Intelligenz, true);
                                break;
                            case AttributeType.Willenskraft:
                                SetDropGestureRecognizerAllow(Willenskraft, true);
                                break;
                            case AttributeType.Charisma:
                                SetDropGestureRecognizerAllow(Charisma, true);
                                break;
                            case AttributeType.Wahrnehmung:
                                SetDropGestureRecognizerAllow(Wahrnehmung, true);
                                break;
                        }
                    }
                }
            }
        }

        private void ResetDropHighlight()
        {
            SetDropGestureRecognizerAllow(Charisma, false);
            SetDropGestureRecognizerAllow(Wahrnehmung, false);
            SetDropGestureRecognizerAllow(Staerke, false);
            SetDropGestureRecognizerAllow(Willenskraft, false);
            SetDropGestureRecognizerAllow(Konstitution, false);
            SetDropGestureRecognizerAllow(Intelligenz, false);
            SetDropGestureRecognizerAllow(Geschicklichkeit, false);
        }

        private void SetDropGestureRecognizerAllow(AttributeExperienceItem view, bool allowValue)
        {
            Debug.WriteLine(view.GestureRecognizers.Count);

            var rec = ((DropGestureRecognizer)view.GestureRecognizers.First(_ =>
                _.GetType() == typeof(DropGestureRecognizer)));
            rec.AllowDrop = allowValue;

            if (allowValue)
            {
                view.BackgroundColor = (Color) Xamarin.Forms.Application.Current.Resources["HellGruenesUmbra2"];
            }
            else
            {
                view.BackgroundColor = (Color)Xamarin.Forms.Application.Current.Resources["AntiUmbra3"];
            }
        }

        private void DropGestureRecognizer_OnDrop(object sender, DropEventArgs e)
        {
            try
            {
                var dropGestureRecognizer = (DropGestureRecognizer)sender;
                var target = (AttributeExperienceItem) dropGestureRecognizer.Parent;
                var skillGroupModelType = (SkillGroupModelType) e.Data.Properties[nameof(SkillGroupModelType)];
                var targetAttributeModel = (AttributeModel) target.BindingContext;
                
                CharacterViewModel.CharacterModel.OpenAttributeIncreases.Remove(skillGroupModelType);

                targetAttributeModel.ExperienceBySkillGroup += 1;

                //force recalc
                targetAttributeModel.TotalExperience = targetAttributeModel.TotalExperience;
                
                ResetDropHighlight();

                //check if all are done
                if (!CharacterViewModel.CharacterModel.OpenAttributeIncreases.Any())
                {
                    CloseCommand?.Execute(null);
                }
            }
            catch (Exception exception)
            {
                App.ErrorManager.TrackException(exception, CharacterViewModel.CharacterModel.Name);
            }
        }

        private void DragGestureRecognizer_OnDropCompleted(object sender, DropCompletedEventArgs e)
        {
            return;
            //drop has been cancelled
            ResetDropHighlight();
        }
    }
}