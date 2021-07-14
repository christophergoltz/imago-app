namespace ImagoApp.Application.Models
{
    public class WikiTabModel : BindableBase
    {
        public WikiTabModel(string url)
        {
            Url = url;
        }

        private string _url;
        private string _title;

        public string Url
        {
            get => _url;
            set => SetProperty(ref _url, value);
        }

        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }
    }
}