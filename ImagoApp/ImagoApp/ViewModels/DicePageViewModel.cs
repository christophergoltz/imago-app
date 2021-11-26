using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows.Input;
using ImagoApp.Application;
using ImagoApp.Application.Models;
using ImagoApp.Application.Services;
using ImagoApp.Shared.Attributes;
using ImagoApp.Shared.Enums;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class DiceSearchModelGroup : List<DiceSearchModel>
    {
        public string Name { get; private set; }

        public DiceSearchModelGroup(string name, List<DiceSearchModel> models) : base(models)
        {
            Name = name;
        }
    }

    public class DiceSearchModel
    {
        public object Value { get; set; }
        public string DisplayText { get; set; }
        public DiceSearchModelType Type { get; set; }
    }

    public enum DiceSearchModelType
    {
        Skill,
        SkillGroup
    }

    public class DicePageViewModel : BindableBase
    {
        public CharacterViewModel CharacterViewModel { get; }
        private readonly IWikiService _wikiService;
        private readonly IWikiDataService _wikiDataService;

        public DicePageViewModel(CharacterViewModel characterViewModel, IWikiService wikiService,
            IWikiDataService wikiDataService)
        {
            CharacterViewModel = characterViewModel;
            CharacterViewModel.LoadoutViewModel.LoadoutValueChanged += (sender, args) => RecalculateFinalDiceValue();

            ConcentrationPerAction = 15;

            _wikiService = wikiService;
            _wikiDataService = wikiDataService;
            _selectableDiceTypes = CreateSearchList();

            Search(string.Empty);
        }

        private List<DiceSearchModelGroup> CreateSearchList()
        {
            var skills = new List<DiceSearchModel>();
            var skillGroups = new List<DiceSearchModel>();

            foreach (var skillGroup in CharacterViewModel.CharacterModel.SkillGroups)
            {
                //skillgroup
                var skillGroupNameAttribute = Util.EnumExtensions.GetAttribute<DisplayTextAttribute>(skillGroup.Type);
                skillGroups.Add(new DiceSearchModel()
                {
                    Value = skillGroup.Type,
                    DisplayText = skillGroupNameAttribute?.Text ?? skillGroup.Type.ToString(),
                    Type = DiceSearchModelType.SkillGroup
                });

                foreach (var skill in skillGroup.Skills)
                {
                    //skill
                    var skillNameAttribute = Util.EnumExtensions.GetAttribute<DisplayTextAttribute>(skill.Type);
                    skills.Add(new DiceSearchModel()
                    {
                        Value = skill.Type,
                        DisplayText = skillNameAttribute?.Text ?? skill.Type.ToString(),
                        Type = DiceSearchModelType.Skill
                    });
                }
            }

            return new List<DiceSearchModelGroup>()
            {
                new DiceSearchModelGroup("Fertigkeit", skills.OrderBy(model => model.DisplayText).ToList()),
                new DiceSearchModelGroup("Fertigkeitsgruppe", skillGroups.OrderBy(model => model.DisplayText).ToList())
            };
        }

        private readonly List<DiceSearchModelGroup> _selectableDiceTypes;
        private List<DiceSearchModelGroup> _searchResults;

        public List<DiceSearchModelGroup> SearchResults
        {
            get => _searchResults;
            set => SetProperty(ref _searchResults, value);
        }

        private HtmlWebViewSource _wikiSource;
        private SkillGroupModel _selectedSkillGroup;
        private SkillViewModel _selectedSkill;
        private WeaveTalentModel _selectedWeaveTalent;
        private TalentViewModel _talentViewModel;
        private MasteryViewModel _masteryViewModel;
        private int _finalDiceValue;
        private bool _searchresultVisible;

        public HtmlWebViewSource WikiSource
        {
            get => _wikiSource;
            set => SetProperty(ref _wikiSource, value);
        }

        public SkillGroupModel SelectedSkillGroup
        {
            get => _selectedSkillGroup;
            private set => SetProperty(ref _selectedSkillGroup, value);
        }


        private ICommand _closeSearchbarCommand;
        private int _diceModification;

        public ICommand CloseSearchbarCommand => _closeSearchbarCommand ?? (_closeSearchbarCommand = new Command(() =>
        {
            SearchresultVisible = false;
        }));

        public bool SearchresultVisible
        {
            get => _searchresultVisible;
            set => SetProperty(ref _searchresultVisible, value);
        }

        public SkillViewModel SelectedSkill
        {
            get => _selectedSkill;
            private set => SetProperty(ref _selectedSkill, value);
        }

        public WeaveTalentModel SelectedWeaveTalent
        {
            get => _selectedWeaveTalent;
            private set => SetProperty(ref _selectedWeaveTalent, value);
        }

        public TalentViewModel TalentViewModel
        {
            get => _talentViewModel;
            set => SetProperty(ref _talentViewModel, value);
        }

        public MasteryViewModel MasteryViewModel
        {
            get => _masteryViewModel;
            set => SetProperty(ref _masteryViewModel, value);
        }

        public int FinalDiceValue
        {
            get => _finalDiceValue;
            set => SetProperty(ref _finalDiceValue, value);
        }

        public int DiceModification
        {
            get => _diceModification;
            set
            {
                SetProperty(ref _diceModification, value);
                RecalculateFinalDiceValue();
            }
        }

        public void SetSelection(DiceSearchModelType type, object value)
        {
            var z = _selectableDiceTypes.SelectMany(group => group)
                .Where(model => model.Type == type)
                .First(model => (SkillModelType) model.Value == (SkillModelType) value);

            SetSelectedItem(z);
        }

        private bool _allDifficultyRemoved;
        private int _concentrationPerAction;
        private int _concentrationQuantity;

        public int ConcentrationPerAction
        {
            get => _concentrationPerAction;
            set
            {
                SetProperty(ref _concentrationPerAction, value);
                OnPropertyChanged(nameof(ConcentrationFinalValue));
                RecalculateFinalDiceValue();
            }
        }

        public int ConcentrationQuantity
        {
            get => _concentrationQuantity;
            set
            {
                SetProperty(ref _concentrationQuantity, value);
                OnPropertyChanged(nameof(ConcentrationFinalValue));
                RecalculateFinalDiceValue();
            }
        }

        public bool AllDifficultyRemoved
        {
            get => _allDifficultyRemoved;
            set
            {
                SetProperty(ref _allDifficultyRemoved, value);
                RecalculateFinalDiceValue();
            }
        }

        public int ConcentrationFinalValue => ConcentrationPerAction * ConcentrationQuantity;


        public void SetSelectedItem(DiceSearchModel selection)
        {
            SearchresultVisible = false;
            DiceModification = 0;

            if (selection == null)
            {
                //reset all
                SelectedSkill = null;
                SelectedSkillGroup = null;
                SelectedWeaveTalent = null;
                TalentViewModel = null;
                MasteryViewModel = null;

                RecalculateFinalDiceValue();
                return;
            }

            if (selection.Value is SkillModelType skillModelType)
            {
                SelectedSkillGroup = null;
                SelectedWeaveTalent = null;


                var group = CharacterViewModel.CharacterModel.SkillGroups.First(groupModel =>
                    groupModel.Skills.Any(e => e.Type == skillModelType));

                var skillModel = group.Skills.First(skill => skill.Type == skillModelType);
                SelectedSkill = new SkillViewModel(skillModel, group, CharacterViewModel);

                var html = _wikiService.GetTalentHtml(skillModelType, group.Type);
                var url = _wikiService.GetWikiUrl(skillModelType);
                WikiSource = new HtmlWebViewSource()
                {
                    BaseUrl = url,
                    Html = html
                };

                TalentViewModel = new TalentViewModel(_wikiDataService.GetAllTalents(SelectedSkill.Skill.Type),
                    CharacterViewModel);
                TalentViewModel.TalentValueChanged += (sender, args) => RecalculateFinalDiceValue();
                MasteryViewModel =
                    new MasteryViewModel(_wikiDataService.GetAllMasteries(group.Type), CharacterViewModel);
                MasteryViewModel.MasteryValueChanged += (sender, args) => RecalculateFinalDiceValue();

                RecalculateFinalDiceValue();
                return;
            }

            if (selection.Value is SkillGroupModelType skillGroupModelType)
            {
                SelectedSkill = null;
                SelectedWeaveTalent = null;
                TalentViewModel = null;

                var skillGroupModel =
                    CharacterViewModel.CharacterModel.SkillGroups.First(group => group.Type == skillGroupModelType);
                SelectedSkillGroup = skillGroupModel;
                var html = _wikiService.GetMasteryHtml(skillGroupModelType);
                var url = _wikiService.GetWikiUrl(skillGroupModelType);
                WikiSource = new HtmlWebViewSource()
                {
                    BaseUrl = url,
                    Html = html
                };
                MasteryViewModel = new MasteryViewModel(_wikiDataService.GetAllMasteries(skillGroupModel.Type),
                    CharacterViewModel);
                MasteryViewModel.MasteryValueChanged += (sender, args) => RecalculateFinalDiceValue();

                RecalculateFinalDiceValue();
                return;
            }
        }

        public void Search(string searchValue)
        {
            if (string.IsNullOrWhiteSpace(searchValue))
            {
                //show all
                var searchResult = new List<DiceSearchModelGroup>();

                //copy list into searchresults to prevent ref removing
                foreach (var group in _selectableDiceTypes)
                {
                    searchResult.Add(new DiceSearchModelGroup(group.Name, group));
                }

                SearchResults = searchResult;
                return;
            }
            
            var result = new List<DiceSearchModelGroup>();
            //copy list into searchresults to prevent ref removing
            foreach (var group in _selectableDiceTypes)
            {
                var newGroup = new DiceSearchModelGroup(group.Name, group);
                foreach (var possibleHit in group)
                {
                    var match = CultureInfo.InvariantCulture.CompareInfo.IndexOf(possibleHit.DisplayText, searchValue,
                        CompareOptions.IgnoreCase) >= 0;

                    if (!match)
                        newGroup.Remove(possibleHit);
                }

                result.Add(newGroup);
            }

            SearchResults = result;
        }

        private void RecalculateFinalDiceValue()
        {
            double value = 0.0;

            if (SelectedSkill != null)
                value = SelectedSkill.Skill.FinalValue;
            if (SelectedSkillGroup != null)
                value = SelectedSkillGroup.FinalValue;

            value -= CharacterViewModel.LoadoutViewModel.GetLoadoutValue();

            if (TalentViewModel != null)
                value -= TalentViewModel.GetDifficultyValue();

            if (MasteryViewModel != null)
                value -= MasteryViewModel.GetDifficultyValue();

            value += DiceModification;
            value += ConcentrationFinalValue;

            FinalDiceValue = value.GetRoundedValue();
        }
    }
}