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
            ResetDropHighlight(true);
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

        private void ResetDropHighlight(bool? visibleOverride = null)
        {
            SetDropGestureRecognizerAllow(Charisma, false, visibleOverride);
            SetDropGestureRecognizerAllow(Wahrnehmung, false, visibleOverride);
            SetDropGestureRecognizerAllow(Staerke, false, visibleOverride);
            SetDropGestureRecognizerAllow(Willenskraft, false, visibleOverride);
            SetDropGestureRecognizerAllow(Konstitution, false, visibleOverride);
            SetDropGestureRecognizerAllow(Intelligenz, false, visibleOverride);
            SetDropGestureRecognizerAllow(Geschicklichkeit, false, visibleOverride);
        }

        private void SetDropGestureRecognizerAllow(AttributeExperienceItem view, bool allowValue, bool? visibleOverride = null)
        {
            if (visibleOverride == null)
            {
                //apply normal value
                view.IsVisible = allowValue;
            }
            else
            {
                view.IsVisible = visibleOverride.Value;
            }

            if (allowValue)
                view.BackgroundColor = (Color)App.GetAppResourcesByName("ConfirmColor");
            else
                view.BackgroundColor = (Color)App.GetAppResourcesByName("SecondaryFirstLightColor");
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

                CharacterViewModel.AddSkillGroupExperienceToAttribute(targetAttributeModel, +1);

                ResetDropHighlight(true);

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
            //drop has been cancelled
            ResetDropHighlight(true);
        }
    }
}