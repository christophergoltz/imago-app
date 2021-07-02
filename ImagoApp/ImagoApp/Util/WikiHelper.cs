using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using HtmlAgilityPack;
using Microsoft.AppCenter.Crashes;

namespace ImagoApp.Util
{
    public static class WikiHelper
    {
        public static HtmlDocument LoadDocumentFromUrl(string url, ObservableCollection<LogEntry> logFeed)
        {
            var htmlWeb = new HtmlWeb();
            var doc = htmlWeb.Load(url);

            if (htmlWeb.StatusCode == HttpStatusCode.NotFound)
            {
                Crashes.TrackError(new InvalidOperationException("HtmlWeb response was 404"),
                    new Dictionary<string, string>() {{"url", url}});

                if (logFeed != null)
                    logFeed.Add(new LogEntry(LogEntryType.Error, $"Seite nicht gefunden \"{url}\""));
                return null;
            }

            return doc;
        }
    }
}