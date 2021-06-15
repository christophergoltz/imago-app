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
        private readonly SkillGroupTypeToAttributeSourceStringConverter _converter = new SkillGroupTypeToAttributeSourceStringConverter();
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
        
        public bool TestAvaiable
        {
            get => _testAvaiable;
            set => SetProperty(ref _testAvaiable, value);
        }

        public SkillDetailViewModel(Skill skill, SkillGroup parent, Character character, ICharacterService characterService, 
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

            CloseCommand = new Command(() =>
            {
                CloseRequested?.Invoke(this, EventArgs.Empty);
            });

            Task.Run(LoadWikiPage);

            if (WikiConstants.ParsableSkillTypeLookUp.ContainsKey(Skill.Type))
            {
                TestAvaiable = true;
                Device.BeginInvokeOnMainThread(() =>
                {
                    Task.Run(InitializeTalents);
                });
            }
            else
            {
                TestAvaiable = false;
            }
        }

        public int FinalTestValue
        {
            get => _finalTestValue;
            set => SetProperty(ref _finalTestValue , value);
        }

        private void RecalcTestValue()
        {
            var result = (int)Skill.FinalValue;

            foreach (var mastery in Masteries)
            {
                if (mastery.Talent.ActiveUse == false || mastery.Talent.ActiveUse && mastery.InUse)
                    result -= mastery.Talent.Difficulty ?? mastery.DifficultyOverride ?? 0;
            }

            foreach (var talent in Talents)
            {
                if (talent.Talent.ActiveUse == false || talent.Talent.ActiveUse && talent.InUse)
                    result -= talent.Talent.Difficulty ?? talent.DifficultyOverride ?? 0;
            }

            FinalTestValue = result;
        }

        public void UpdateTalentRequirements()
        {
            foreach (var mastery in Masteries)
            {
                if (mastery.Talent is MasteryModel model)
                {
                    var avaiable = _characterService.CheckMasteryRequirement(model.Requirements, _character);
                    mastery.Talent.Available = avaiable;
                }
            }

            foreach (var talent in Talents)
            {
                if (talent.Talent is TalentModel model)
                {
                    var avaiable = _characterService.CheckTalentRequirement(model.Requirements, _character);
                    talent.Talent.Available = avaiable;
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
                    mastery.Available = avaiable;

                    var vm = new TalentListItemViewModel(mastery);
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
                    talent.Available = avaiable;

                    var vm = new TalentListItemViewModel(talent);
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