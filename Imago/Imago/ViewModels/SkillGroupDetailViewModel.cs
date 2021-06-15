using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Imago.Converter;
using Imago.Models;
using Imago.Models.Base;
using Imago.Services;
using Imago.Util;
using Imago.Views;
using Xamarin.Forms;

namespace Imago.ViewModels
{
    public class SkillGroupDetailViewModel : BindableBase
    {
        private readonly SkillGroupTypeToAttributeSourceStringConverter _converter = new SkillGroupTypeToAttributeSourceStringConverter();

        private readonly ICharacterService _characterService;
        private readonly IWikiService _wikiService;
        public SkillGroup SkillGroup { get; }
        public event EventHandler CloseRequested;
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
            get => SkillGroup?.ModificationValue ?? 0;
            set
            {
                Debug.WriteLine("Set SelectedSkillModification to " + value);
                _characterService.SetModificationValue(SkillGroup, value);
                OnPropertyChanged(nameof(SelectedSkillModification));
            }
        }

        public SkillGroupDetailViewModel(SkillGroup skillGroup, ICharacterService characterService, IWikiService wikiService)
        {
            _characterService = characterService;
            _wikiService = wikiService;
            SkillGroup = skillGroup;
            SourceFormula = _converter.Convert(skillGroup.Type, null, null, CultureInfo.InvariantCulture).ToString();

            OpenWikiCommand = new Command(async () =>
            {
                var url = wikiService.GetWikiUrl(skillGroup.Type);

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
            var html = _wikiService.GetMasteryHtml(SkillGroup.Type);
            var url = _wikiService.GetWikiUrl(SkillGroup.Type);
            QuickWikiView = new HtmlWebViewSource()
            {
                BaseUrl = url,
                Html = html
            };
        }
    }
}