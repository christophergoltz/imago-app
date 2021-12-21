using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using ImagoApp.Application;
using ImagoApp.Application.Services;
using ImagoApp.Shared.Attributes;
using ImagoApp.Shared.Enums;

namespace ImagoApp.ViewModels.Dialog
{
    public class DiceSearchDialogViewModel : BindableBase
    {
        private readonly CharacterViewModel _characterViewModel;
        private readonly IWikiDataService _wikiDataService;
        public event EventHandler<DiceSearchModel> OnSearchCompleted;

        public DiceSearchDialogViewModel(CharacterViewModel characterViewModel, IWikiDataService wikiDataService)
        {
            _characterViewModel = characterViewModel;
            _wikiDataService = wikiDataService;

            _allSelectableDiceTypes = CreateSearchList();

            SearchText = string.Empty;
        }

        public void SetSelectedItem(DiceSearchModel searchModel)
        {
            OnSearchCompleted?.Invoke(this, searchModel);
        }

        private readonly List<DiceSearchModelGroup> _allSelectableDiceTypes;

        private List<DiceSearchModelGroup> _searchResults;
        private string _searchText;

        public List<DiceSearchModelGroup> SearchResults
        {
            get => _searchResults;
            set => SetProperty(ref _searchResults, value);
        }

        private List<DiceSearchModelGroup> CreateSearchList()
        {
            var skills = new List<DiceSearchModel>();
            var skillGroups = new List<DiceSearchModel>();

            //skills and skillgroups
            foreach (var skillGroup in _characterViewModel.CharacterModel.SkillGroups)
            {
                //skillgroup
                var skillGroupNameAttribute = Util.EnumExtensions.GetAttribute<DisplayTextAttribute>(skillGroup.Type);
                skillGroups.Add(new DiceSearchModel(
                    skillGroup.Type,
                    skillGroupNameAttribute?.Text ??
                    skillGroup.Type.ToString(),
                    null,
                    DiceSearchModelType.SkillGroup));

                foreach (var skill in skillGroup.Skills)
                {
                    //skill
                    var skillNameAttribute = Util.EnumExtensions.GetAttribute<DisplayTextAttribute>(skill.Type);
                    skills.Add(new DiceSearchModel(
                        skill.Type,
                        skillNameAttribute?.Text ?? skill.Type.ToString(),
                        null,
                        DiceSearchModelType.Skill));
                }
            }

            var result = new List<DiceSearchModelGroup>()
            {
                new DiceSearchModelGroup("Fertigkeit", DiceSearchModelType.Skill, skills.OrderBy(model => model.DisplayText).ToList()),
                new DiceSearchModelGroup("Fertigkeitsgruppe", DiceSearchModelType.SkillGroup, skillGroups.OrderBy(model => model.DisplayText).ToList())
            };

            //weave talents
            var allWeaveTalents = _wikiDataService.GetAllWeaveTalents();
            var weaveTalentGroups = allWeaveTalents.GroupBy(model => model.WeaveSource).ToList();
            foreach (var weaveTalentGroup in weaveTalentGroups)
            {
                //group
                var weaveTalents = new List<DiceSearchModel>();

                foreach (var weaveTalent in weaveTalentGroup)
                {
                    //talent
                    var available = _characterViewModel.CheckTalentRequirement(weaveTalent.Requirements);
                    if (!available)
                        continue;

                    var weaveForm = weaveTalent.Requirements.Count == 3;

                    weaveTalents.Add(new DiceSearchModel(
                        weaveTalent,
                        weaveTalent.Name,
                        weaveTalent.ShortDescription,
                        weaveForm ? DiceSearchModelType.WeaveTalentMultiple : DiceSearchModelType.WeaveTalent
                        ));
                }

                if (weaveTalents.Any())
                {
                    result.Add(new DiceSearchModelGroup(weaveTalentGroup.Key, weaveTalents.First().Type, weaveTalents));
                }
            }

            return result;
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                SetProperty(ref _searchText, value);
                Search(value);
            }
        }

        public void SetSelection(DiceSearchModelType type, object value)
        {
            var selectionResult = _allSelectableDiceTypes.SelectMany(group => group)
                .Where(model => model.Type == type)
                .First(model => (SkillModelType)model.Value == (SkillModelType)value);

            OnSearchCompleted?.Invoke(this, selectionResult);
        }

        private void Search(string searchValue)
        {
            if (string.IsNullOrWhiteSpace(searchValue))
            {
                //show all
                var searchResult = new List<DiceSearchModelGroup>();

                //copy list into searchresults to prevent ref removing
                foreach (var group in _allSelectableDiceTypes)
                {
                    searchResult.Add(new DiceSearchModelGroup(group.Name, group.Type, group));
                }

                SearchResults = searchResult;
                return;
            }

            var result = new List<DiceSearchModelGroup>();
            //copy list into searchresults to prevent ref removing
            foreach (var group in _allSelectableDiceTypes)
            {
                var newGroup = new DiceSearchModelGroup(group.Name, group.Type, group);
                foreach (var possibleHit in group)
                {
                    var match = CultureInfo.InvariantCulture.CompareInfo.IndexOf(possibleHit.DisplayText, searchValue,
                        CompareOptions.IgnoreCase) >= 0;

                    if (!match)
                        newGroup.Remove(possibleHit);
                }

                if (newGroup.Any())
                    result.Add(newGroup);
            }

            SearchResults = result;
        }
    }
}
