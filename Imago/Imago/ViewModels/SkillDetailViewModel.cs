using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using Imago.Converter;
using Imago.Models;
using Imago.Models.Enum;
using Imago.Repository.WrappingDatabase;
using Imago.Services;
using Imago.Util;
using Imago.Views;
using Xamarin.Forms;

namespace Imago.ViewModels
{
    public class SkillDetailViewModel : BindableBase
    {
        private readonly SkillGroupTypeToAttributeSourceStringConverter _converter =
            new SkillGroupTypeToAttributeSourceStringConverter();

        private readonly SkillGroup _parent;
        private readonly Character _character;
        private readonly ICharacterService _characterService;
        private readonly IWikiService _wikiService;
        private readonly IMasteryRepository _masteryRepository;
        private readonly ITalentRepository _talentRepository;

        public event EventHandler CloseRequested;
        public Skill Skill { get; }

        public ICommand IncreaseExperienceCommand { get; set; }
        public ICommand IncreaseExperienceByFiveCommand { get; set; }
        public ICommand DecreaseExperienceCommand { get; set; }
        public ICommand OpenWikiCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        private HtmlWebViewSource _quickWikiView;

        public HtmlWebViewSource QuickWikiView
        {
            get => _quickWikiView;
            set => SetProperty(ref _quickWikiView, value);
        }

        private string _sourceFormula;
        private List<TalentListItemViewModel> _talents;
        private List<TalentListItemViewModel> _masteries;
        private bool _testAvaiable = false;
        private int _finalTestValue;
        private DerivedAttributeType _selectedHandicap;

        public string SourceFormula
        {
            get => _sourceFormula;
            set => SetProperty(ref _sourceFormula, value);
        }

        public int SelectedSkillModification
        {
            get => Skill?.ModificationValue ?? 0;
            set
            {
                Debug.WriteLine("Set SelectedSkillModification to " + value);
                _characterService.SetModificationValue(Skill, value);
                OnPropertyChanged(nameof(SelectedSkillModification));
            }
        }

        public List<TalentListItemViewModel> Talents
        {
            get => _talents;
            set => SetProperty(ref _talents, value);
        }

        public List<TalentListItemViewModel> Masteries
        {
            get => _masteries;
            set => SetProperty(ref _masteries, value);
        }
        
        public DerivedAttributeType SelectedHandicap
        {
            get => _selectedHandicap;
            set
            {
                SetProperty(ref _selectedHandicap, value);
                RecalcTestValue();
            }
        }

        public int HandicapValueFight => (int)_character.Handicap.First(attribute => attribute.Type == DerivedAttributeType.BehinderungKampf).FinalValue;
        public int HandicapValueAdventure => (int)_character.Handicap.First(attribute => attribute.Type == DerivedAttributeType.BehinderungAbenteuer).FinalValue;
        public int HandicapValueTotal => (int)_character.Handicap.First(attribute => attribute.Type == DerivedAttributeType.BehinderungGesamt).FinalValue;

        public SkillDetailViewModel(Skill skill, SkillGroup parent, Character character,
            ICharacterService characterService,
            IWikiService wikiService, IMasteryRepository masteryRepository, ITalentRepository talentRepository)
        {
            _parent = parent;
            _character = character;
            _characterService = characterService;
            _wikiService = wikiService;
            _masteryRepository = masteryRepository;
            _talentRepository = talentRepository;
            Skill = skill;

            SourceFormula = _converter.Convert(parent.Type, null, null, CultureInfo.InvariantCulture).ToString();

            IncreaseExperienceCommand = new Command(() =>
            {
                var newOpenAttributeIncreases =
                    characterService.AddOneExperienceToSkill(Skill, parent).ToList();
                if (newOpenAttributeIncreases.Any())
                {
                    foreach (var increase in newOpenAttributeIncreases)
                    {
                        character.OpenAttributeIncreases.Add(increase);
                    }
                }

                UpdateTalentRequirements();
                RecalcTestValue();
            });

            IncreaseExperienceByFiveCommand = new Command(() =>
            {
                var newOpenAttributeIncreases = new List<SkillGroupType>();
                for (var i = 0; i < 5; i++)
                {
                    newOpenAttributeIncreases.AddRange(characterService.AddOneExperienceToSkill(Skill, parent));
                }

                if (newOpenAttributeIncreases.Any())
                {
                    foreach (var increase in newOpenAttributeIncreases)
                    {
                        character.OpenAttributeIncreases.Add(increase);
                    }
                }

                UpdateTalentRequirements();
                RecalcTestValue();
            });

            DecreaseExperienceCommand = new Command(() => { characterService.RemoveOneExperienceFromSkill(skill); });

            OpenWikiCommand = new Command(async () =>
            {
                var url = wikiService.GetWikiUrl(skill.Type);

                if (string.IsNullOrWhiteSpace(url))
                {
                    await Application.Current.MainPage.DisplayAlert("Fehlender Link",
                        "Uups, hier ist wohl nichts hinterlegt..", "OK");
                    return;
                }

                WikiPageViewModel.RequestedWikiPage = new WikiPageEntry(url);
                await Shell.Current.GoToAsync($"//{nameof(WikiPage)}");
                WikiPageViewModel.RequestedWikiPage = null;
            });

            CloseCommand = new Command(() => { CloseRequested?.Invoke(this, EventArgs.Empty); });

            Task.Run(LoadWikiPage);
            Task.Run(InitializeTalents);

            SelectedHandicap = DerivedAttributeType.BehinderungAbenteuer;
        }

        public int FinalTestValue
        {
            get => _finalTestValue;
            set => SetProperty(ref _finalTestValue, value);
        }

        private void RecalcTestValue()
        {
            var result = (int) Skill.FinalValue;

            //handicap
            if (SelectedHandicap != DerivedAttributeType.Unknown)
            {
                var val = _character.Handicap.First(attribute => attribute.Type == SelectedHandicap)
                    .FinalValue;
                result -= (int) val;
            }

            //masteries
            if (Masteries != null)
            {
                foreach (var mastery in Masteries)
                {
                    if (!mastery.Available)
                        continue;

                    if (mastery.Talent.ActiveUse == false || mastery.Talent.ActiveUse && mastery.InUse)
                        result -= mastery.Talent.Difficulty ?? mastery.DifficultyOverride ?? 0;
                }
            }

            //talents
            if (Talents != null)
            {
                foreach (var talent in Talents)
                {
                    if (!talent.Available)
                        continue;

                    if (talent.Talent.ActiveUse == false || talent.Talent.ActiveUse && talent.InUse)
                        result -= talent.Talent.Difficulty ?? talent.DifficultyOverride ?? 0;
                }
            }

            FinalTestValue = result;
        }

        public void UpdateTalentRequirements()
        {
            if (Masteries != null)
            {
                foreach (var mastery in Masteries)
                {
                    if (mastery.Talent is MasteryModel model)
                    {
                        var avaiable = _characterService.CheckMasteryRequirement(model.Requirements, _character);
                        mastery.Available = avaiable;
                    }
                }
            }

            if(Talents != null)
            {
                foreach (var talent in Talents)
                {
                    if (talent.Talent is TalentModel model)
                    {
                        var avaiable = _characterService.CheckTalentRequirement(model.Requirements, _character);
                        talent.Available = avaiable;
                    }
                }
            }
        }

        private async Task InitializeTalents()
        {
            var masteries = new List<TalentListItemViewModel>();
            var allMasteries = await _masteryRepository.GetAllItemsAsync();
            foreach (var mastery in allMasteries)
            {
                if (mastery.TargetSkill != _parent.Type)
                    continue;

                var avaiable = _characterService.CheckMasteryRequirement(mastery.Requirements, _character);

                var vm = new TalentListItemViewModel(mastery)
                {
                    Available = avaiable
                };
                vm.TalentValueChanged += (sender, args) => RecalcTestValue();
                masteries.Add(vm);
            }

            Masteries = masteries;

            var talents = new List<TalentListItemViewModel>();
            var allTalents = await _talentRepository.GetAllItemsAsync();
            foreach (var talent in allTalents)
            {
                if (talent.TargetSkill != Skill.Type)
                    continue;

                var avaiable = _characterService.CheckTalentRequirement(talent.Requirements, _character);

                var vm = new TalentListItemViewModel(talent)
                {
                    Available = avaiable
                };
                vm.TalentValueChanged += (sender, args) => RecalcTestValue();
                talents.Add(vm);
            }

            Talents = talents;

            UpdateTalentRequirements();
            RecalcTestValue();
        }

        private void LoadWikiPage()
        {
            var html = _wikiService.GetTalentHtml(Skill.Type);
            var url = _wikiService.GetWikiUrl(Skill.Type);
            QuickWikiView = new HtmlWebViewSource()
            {
                BaseUrl = url,
                Html = html
            };
        }
    }
}