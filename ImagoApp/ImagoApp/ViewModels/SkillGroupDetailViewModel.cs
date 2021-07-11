using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;
using ImagoApp.Application;
using ImagoApp.Application.Models;
using ImagoApp.Application.Services;
using ImagoApp.Converter;
using ImagoApp.Util;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class SkillGroupDetailViewModel : BindableBase
    {
        private readonly SkillGroupTypeToAttributeSourceStringConverter _converter =
            new SkillGroupTypeToAttributeSourceStringConverter();

        private readonly CharacterViewModel _characterViewModel;
        private readonly IWikiService _wikiService;
        public SkillGroupModel SkillGroupModel { get; }
        public event EventHandler CloseRequested;
        public ICommand OpenWikiCommand { get; set; }
        public ICommand CloseCommand { get; set; }

        private HtmlWebViewSource _quickWikiView;
        public event EventHandler<string> OpenWikiPageRequested;

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
                    await Xamarin.Forms.Application.Current.MainPage.DisplayAlert("Fehlender Link",
                        "Uups, hier ist wohl nichts hinterlegt..", "OK");
                    return;
                }

                OpenWikiPageRequested?.Invoke(this, url);
            });

            CloseCommand = new Command(() => { CloseRequested?.Invoke(this, EventArgs.Empty); });

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