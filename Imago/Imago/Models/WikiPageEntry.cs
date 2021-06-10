using System;
using System.Collections.Generic;
using System.Text;
using Imago.Util;

namespace Imago.Models
{
    public class WikiPageEntry : BindableBase
    {
        public WikiPageEntry(string url)
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