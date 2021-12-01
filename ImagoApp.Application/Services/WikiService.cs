using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using HtmlAgilityPack;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Application.Services
{
    public interface IWikiService
    {
        string GetWikiUrl(SkillModelType skillModelType);
        string GetWikiUrl(SkillGroupModelType skillGroupModelType);
        string GetTalentHtml(SkillModelType skillModelType, SkillGroupModelType skillGroupModelType);
        string GetMasteryHtml(SkillGroupModelType skillGroupModelType);
    }

    public class WikiService : IWikiService
    {
        public string GetTalentHtml(SkillModelType skillModelType, SkillGroupModelType skillGroupModelType)
        {
            var url = WikiConstants.SkillTypeLookUp[skillModelType];
            var titlesToRemove = new List<string>();

            if (skillGroupModelType == SkillGroupModelType.Nahkampf ||
                skillGroupModelType == SkillGroupModelType.Fernkampf)
            {
                titlesToRemove.Add("Künste");
                titlesToRemove.Add("Variationen");
            }
               
            if (skillGroupModelType == SkillGroupModelType.Webkunst)
                titlesToRemove.Add("Beinamen");
            
            return GetHtmlDocumentContent(url, true, titlesToRemove.ToArray());
        }

        public string GetMasteryHtml(SkillGroupModelType skillGroupModelType)
        {
            var url = WikiConstants.SkillGroupTypeLookUp[skillGroupModelType];
            return GetHtmlDocumentContent(url, true);
        }
        
        private string GetHtmlDocumentContent(string url, bool killLinks, params string[] titlesToRemove)
        {
            //todo logger null
            var document = WikiHelper.LoadDocumentFromUrl(url, null);
            if (document == null)
                return string.Empty;

            document.GetElementbyId("mw-page-base")?.Remove();
            document.GetElementbyId("mw-head-base")?.Remove();
            document.GetElementbyId("mw-navigation")?.Remove();
            document.GetElementbyId("footer")?.Remove();
            document.GetElementbyId("catlinks")?.Remove();
            document.GetElementbyId("toc")?.Remove();
            document.GetElementbyId("jump-to-nav")?.Remove();
            document.GetElementbyId("mw-notification-area")?.Remove();
            document.GetElementbyId("firstHeading")?.Remove();
            document.GetElementbyId("siteSub")?.Remove();
            document.GetElementbyId("contentSub")?.Remove();

            //reduced left margin created by side menu
            document.GetElementbyId("content")?.SetAttributeValue("style", "margin-left: 0px;");

            if (killLinks)
            {
                //kill all links
                while (document.DocumentNode.Descendants("a").FirstOrDefault() != null)
                {
                    var parent = document.DocumentNode.Descendants("a").First().ParentNode;

                    if (string.IsNullOrWhiteSpace(parent.InnerHtml))
                        continue;

                    parent.InnerHtml = parent.InnerHtml.Replace("<a", "<span").Replace("</a", "</span");
                }
            }

            if (titlesToRemove.Any())
            {
                var htmlNodesToRemove = new List<HtmlNode>();

                var headLines = document.DocumentNode.SelectNodes("//span[@class='mw-headline']");
                if (headLines != null)
                {
                    foreach (var headLine in headLines)
                    {
                        var header = CleanUpString(headLine.InnerText);

                        if (!titlesToRemove.Contains(header))
                            continue;

                        var parent = headLine.ParentNode;
                        htmlNodesToRemove.Add(parent);

                        var next = parent?.NextSibling;
                        if (next == null)
                            continue;

                        //remove all following until next h2
                        while (!next.Name.Equals("h2"))
                        {
                            htmlNodesToRemove.Add(next);
                            next = next.NextSibling;
                            if (next == null)
                                break;
                        }
                    }

                    foreach (var htmlNode in htmlNodesToRemove)
                    {
                        htmlNode.Remove();
                    }
                }
            }

            return document.DocumentNode.OuterHtml;
        }


        public string GetWikiUrl(SkillModelType skillModelType)
        {
            if (WikiConstants.SkillTypeLookUp.ContainsKey(skillModelType))
                return WikiConstants.SkillTypeLookUp[skillModelType];

            return string.Empty;
        }

        public string GetWikiUrl(SkillGroupModelType skillGroupModelType)
        {
            if (WikiConstants.SkillGroupTypeLookUp.ContainsKey(skillGroupModelType))
                return WikiConstants.SkillGroupTypeLookUp[skillGroupModelType];

            return string.Empty;
        }

        private string CleanUpString(string value)
        {
            return value.Replace("\n", "").Replace("\r", "").Trim();
        }
    }
}