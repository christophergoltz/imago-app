using System;
using System.Globalization;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ImagoApp.Application;
using ImagoApp.Application.Models;
using ImagoApp.Application.Services;
using ImagoApp.Converter;
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
        

        private ICommand _closeCommand;

        public ICommand CloseCommand => _closeCommand ?? (_closeCommand = new Command(() =>
        {
            try
            {
                CloseRequested?.Invoke(this, EventArgs.Empty);
            }
            catch (Exception exception)
            {
                App.ErrorManager.TrackException(exception, _characterViewModel.CharacterModel.Name);
            }
        }));
        
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
                _characterViewModel.SetModification(SkillGroupModel, value);
                OnPropertyChanged(nameof(SelectedSkillModification));
            }
        }

        public SkillGroupDetailViewModel(SkillGroupModel skillGroupModel, CharacterViewModel characterViewModel, IWikiService wikiService)
        {
            _characterViewModel = characterViewModel;
            _wikiService = wikiService;
            SkillGroupModel = skillGroupModel;
            SourceFormula = _converter.Convert(skillGroupModel.Type, null, null, CultureInfo.InvariantCulture).ToString();
            
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