using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net;
using HtmlAgilityPack;
using Microsoft.AppCenter.Crashes;
using Microsoft.Extensions.Logging;
using Serilog.Core;

namespace ImagoApp.Util
{
    public static class WikiHelper
    {
        public static HtmlDocument LoadDocumentFromUrl(string url, Logger logger)
        {
            var htmlWeb = new HtmlWeb();
            var doc = htmlWeb.Load(url);

            if (htmlWeb.StatusCode == HttpStatusCode.NotFound)
            {
                Crashes.TrackError(new InvalidOperationException("HtmlWeb response was 404"),
                    new Dictionary<string, string>() {{"url", url}});

                logger.Error($"Seite nicht gefunden \"{url}\"");
                return null;
            }

            return doc;
        }
    }
}