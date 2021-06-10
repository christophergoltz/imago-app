using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Imago.Shared.Util;
using Imago.Wiki.Parser.Services;
using Imago.Wiki.Parser.Util;
using Newtonsoft.Json;

namespace Imago.Wiki.Parser.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly IWikiParseService _wikiParseService;
        private string _jsonArmorValue;

        public ICommand ParseArmorCommand { get; set; }

        public MainWindowViewModel(IWikiParseService wikiParseService)
        {
            _wikiParseService = wikiParseService;

            ParseArmorCommand = new DelegateCommand(obj =>
            {
          

                var result = _wikiParseService.ParseArmorFromUrl(url);
                var json = JsonConvert.SerializeObject(result);
                JsonArmorValue = json;
            });
        }

        public string JsonArmorValue
        {
            get => _jsonArmorValue;
            set => SetProperty(ref _jsonArmorValue , value);
        }
    }
}
