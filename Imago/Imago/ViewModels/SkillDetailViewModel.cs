using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Imago.Converter;
using Imago.Models;
using Imago.Models.Enum;
using Imago.Services;
using Imago.Util;
using Imago.Views;
using Xamarin.Forms;

namespace Imago.ViewModels
{
    public class SkillDetailViewModel : BindableBase
    {
        private readonly SkillGroupTypeToAttributeSourceStringConverter _converter = new SkillGroupTypeToAttributeSourceStringConverter();
        private readonly ICharacterService _characterService;
        private readonly IWikiService _wikiService;

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

        public SkillDetailViewModel(Skill skill, SkillGroup parent, Character character,
            ICharacterService characterService, IWikiService wikiService)
        {
            _characterService = characterService;
            _wikiService = wikiService;
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