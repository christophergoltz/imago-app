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

        private readonly CharacterViewModel _characterViewModel;
        private readonly IWikiService _wikiService;
        public SkillGroupModel SkillGroupModel { get; }
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
            get => SkillGroupModel?.ModificationValue ?? 0;
            set
            {
                Debug.WriteLine("Set SelectedSkillModification to " + value);
                _characterViewModel.SetModificationValue(SkillGroupModel, value);
                OnPropertyChanged(nameof(SelectedSkillModification));
            }
        }

        public SkillGroupDetailViewModel(SkillGroupModel skillGroupModel, CharacterViewModel characterViewModel, IWikiService wikiService)
        {
            _characterViewModel = characterViewModel;
            _wikiService = wikiService;
            SkillGroupModel = skillGroupModel;
            SourceFormula = _converter.Convert(skillGroupModel.Type, null, null, CultureInfo.InvariantCulture).ToString();

            OpenWikiCommand = new Command(async () =>
            {
                var url = wikiService.GetWikiUrl(skillGroupModel.Type);

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
            var html = _wikiService.GetMasteryHtml(SkillGroupModel.Type);
            var url = _wikiService.GetWikiUrl(SkillGroupModel.Type);
            QuickWikiView = new HtmlWebViewSource()
            {
                BaseUrl = url,
                Html = html
            };
        }
    }
}