using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ImagoApp.Application;
using ImagoApp.Application.Models;
using ImagoApp.Application.Services;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class SkillGroupViewModel : BindableBase
    {
        private readonly IWikiService _wikiService;
        private SkillGroupModel _skillGroup;
        private List<SkillViewModel> _skills;

        public SkillGroupModel SkillGroup
        {
            get => _skillGroup;
            private set => SetProperty(ref _skillGroup ,value);
        }

        public CharacterViewModel CharacterViewModel { get; }

        public List<SkillViewModel> Skills
        {
            get => _skills;
            private set => SetProperty(ref _skills , value);
        }

        public SkillViewModel SelectedSkill
        {
            get => _selectedSkill;
            set
            {
                SetProperty(ref _selectedSkill, value);
                OnPropertyChanged(nameof(SelectedSkillModification));
                Task.Run(LoadWikiPage);
            }
        }

        public SkillGroupViewModel(IWikiService wikiService, SkillGroupModel skillGroup, CharacterViewModel characterViewModel)
        {
            _wikiService = wikiService;
            SkillGroup = skillGroup;
            CharacterViewModel = characterViewModel;
            Skills = skillGroup.Skills.Select(model => new SkillViewModel(model, skillGroup, CharacterViewModel)).ToList();

            SelectedSkill = Skills.First();
        }

        private ICommand _increaseExperienceCommand;

        public ICommand IncreaseExperienceCommand => _increaseExperienceCommand ?? (_increaseExperienceCommand = new Command<int>(experienceValue =>
        {
            try
            {
                CharacterViewModel.AddExperienceToSkill(SelectedSkill.Skill, SkillGroup, experienceValue);
            }
            catch (Exception exception)
            {
                App.ErrorManager.TrackException(exception, CharacterViewModel.CharacterModel.Name, new Dictionary<string, string>()
                {
                    { "Experience Value", experienceValue.ToString()}
                });
            }
        }));

        private ICommand _decreaseExperienceCommand;
        public ICommand DecreaseExperienceCommand => _decreaseExperienceCommand ?? (_decreaseExperienceCommand = new Command(() =>
        {
            try
            {
                //todo -1 by parameter; 
                CharacterViewModel.AddExperienceToSkill(SelectedSkill.Skill, SkillGroup, -1);
            }
            catch (Exception exception)
            {
                App.ErrorManager.TrackException(exception, CharacterViewModel.CharacterModel.Name);
            }
        }));

        private ICommand _openSkillWikiCommand;

        public ICommand OpenSkillWikiCommand => _openSkillWikiCommand ?? (_openSkillWikiCommand = new Command(() =>
        {
            try
            {
                var url = _wikiService.GetWikiUrl(SelectedSkill.Skill.Type);

                if (string.IsNullOrWhiteSpace(url))
                {
                    App.ErrorManager.TrackExceptionSilent(new InvalidOperationException($"No url found for {SelectedSkill.Skill.Type}"));
                    UserDialogs.Instance.Alert($"Uups, für {SelectedSkill.Skill.Type} ist wohl nichts hinterlegt..", "Fehlender Link", "OK");
                    return;
                }

                OpenWikiPageRequested?.Invoke(this, url);
            }
            catch (Exception exception)
            {
                App.ErrorManager.TrackException(exception, CharacterViewModel.CharacterModel.Name);
            }
        }));

        public event EventHandler<string> OpenWikiPageRequested;

        public int SkillGroupModificationValue
        {
            get => SkillGroup?.ModificationValue ?? 0;
            set
            {
                CharacterViewModel.SetModification(SkillGroup, value);
                OnPropertyChanged(nameof(SkillGroupModificationValue));
            }
        }

        private ICommand _openSkillGroupWikiCommand;
        public ICommand OpenSkillGroupWikiCommand => _openSkillGroupWikiCommand ?? (_openSkillGroupWikiCommand = new Command(() =>
        {
            try
            {
                var url = _wikiService.GetWikiUrl(SkillGroup.Type);

                if (string.IsNullOrWhiteSpace(url))
                {
                    App.ErrorManager.TrackExceptionSilent(new InvalidOperationException($"No url found for {SkillGroup.Type}"));
                    UserDialogs.Instance.Alert($"Uups, für {SkillGroup.Type} ist wohl nichts hinterlegt..", "Fehlender Link", "OK");
                    return;
                }

                OpenWikiPageRequested?.Invoke(this, url);
            }
            catch (Exception exception)
            {
                App.ErrorManager.TrackException(exception, CharacterViewModel.CharacterModel.Name);
            }
        }));

        private HtmlWebViewSource _skillWikiSource;
        private SkillViewModel _selectedSkill;

        public HtmlWebViewSource SkillWikiSource
        {
            get => _skillWikiSource;
            set => SetProperty(ref _skillWikiSource, value);
        }

        public int SelectedSkillModification
        {
            get => SelectedSkill?.Skill?.ModificationValue ?? 0;
            set
            {
                CharacterViewModel.SetModification(SelectedSkill.Skill, value);
                OnPropertyChanged(nameof(SelectedSkillModification));
            }
        }

        private void LoadWikiPage()
        {
            var html = _wikiService.GetTalentHtml(SelectedSkill.Skill.Type, SkillGroup.Type);
            var url = _wikiService.GetWikiUrl(SelectedSkill.Skill.Type);
            SkillWikiSource = new HtmlWebViewSource()
            {
                BaseUrl = url,
                Html = html
            };
        }
    }
}
