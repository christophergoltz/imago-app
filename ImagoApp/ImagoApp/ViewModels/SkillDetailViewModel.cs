using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using Acr.UserDialogs;
using ImagoApp.Application;
using ImagoApp.Application.Constants;
using ImagoApp.Application.Models;
using ImagoApp.Application.Services;
using ImagoApp.Converter;
using ImagoApp.Shared.Enums;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class SkillDetailViewModel : BindableBase
    {
        private readonly SkillGroupTypeToAttributeSourceStringConverter _converter =
            new SkillGroupTypeToAttributeSourceStringConverter();

        private readonly SkillGroupModel _parent;
        private readonly CharacterViewModel _characterViewModel;
        private readonly IWikiService _wikiService;
        private readonly IWikiDataService _wikiDataService;

        public event EventHandler CloseRequested;
        public SkillModel SkillModel { get; }

        public event EventHandler<string> OpenWikiPageRequested;

      

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
        private int _finalTestValue;
        private List<HandicapListViewItemViewModel> _handicaps;

        public string SourceFormula
        {
            get => _sourceFormula;
            set => SetProperty(ref _sourceFormula, value);
        }

        public int SelectedSkillModification
        {
            get => SkillModel?.ModificationValue ?? 0;
            set
            {
                _characterViewModel.SetModification(SkillModel, value);
                OnPropertyChanged(nameof(SelectedSkillModification));
            }
        }
        
        public List<HandicapListViewItemViewModel> Handicaps
        {
            get => _handicaps;
            set => SetProperty(ref _handicaps, value);
        }

        public SkillDetailViewModel(SkillModel skillModel, SkillGroupModel parent, CharacterViewModel characterViewModel,
            IWikiService wikiService, IWikiDataService wikiDataService)
        {
            _parent = parent;
            _characterViewModel = characterViewModel;
            _wikiService = wikiService;
            _wikiDataService = wikiDataService;
            SkillModel = skillModel;

            SourceFormula = _converter.Convert(parent.Type, null, null, CultureInfo.InvariantCulture).ToString();
            
            Task.Run(LoadWikiPage);
        }

        public int FinalTestValue
        {
            get => _finalTestValue;
            set => SetProperty(ref _finalTestValue, value);
        }
        
        private static readonly List<(DerivedAttributeType Type, string Text, string IconSource)> HandicapDefinition =
            new List<(DerivedAttributeType Type, string Text, string IconSource)>()
            {
                (DerivedAttributeType.BehinderungKampf, "Kampf", "Images/kampf.png"),
                (DerivedAttributeType.BehinderungAbenteuer, "Abenteuer / Reise", "Images/inventar.png"),
                (DerivedAttributeType.BehinderungGesamt, "Gesamt", null),
                (DerivedAttributeType.Unknown, "Ignorieren", null)
            };

        private void LoadWikiPage()
        {
            var html = _wikiService.GetTalentHtml(SkillModel.Type, _parent.Type);
            var url = _wikiService.GetWikiUrl(SkillModel.Type);
            QuickWikiView = new HtmlWebViewSource()
            {
                BaseUrl = url,
                Html = html
            };
        }
    }
}