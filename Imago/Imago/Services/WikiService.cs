using System.Collections.Generic;
using System.Linq;
using HtmlAgilityPack;
using Imago.Models;
using Imago.Models.Enum;
using Imago.Util;

namespace Imago.Services
{
    public interface IWikiService
    {
        string GetWikiUrl(SkillType skillType);
        string GetWikiUrl(SkillGroupType skillGroupType);
        string GetTalentHtml(SkillType skillType);
        string GetMasteryHtml(SkillGroupType skillGroupType);
        string GetChangelogHtml(List<string> filterHtmlTags);
        string GetChangelogUrl();
    }

    public class WikiService : IWikiService
    {
        public string GetTalentHtml(SkillType skillType)
        {
            var url = WikiConstants.SkillTypeLookUp[skillType];
            return GetHtml(url);
        }

        public string GetChangelogHtml(List<string> filterHtmlTags)
        {
            var url = WikiConstants.ChangelogUrl;
            return GetHtml(url, filterHtmlTags.ToArray());
        }

        public string GetChangelogUrl()
        {
            return WikiConstants.ChangelogUrl;
        }

        private string GetHtml(string url, params string[] filterHtmlTags)
        {
            var web = new HtmlWeb();
            var document = web.Load(url);
            
            document.GetElementbyId("mw-page-base")?.Remove();
            document.GetElementbyId("mw-head-base")?.Remove();
            document.GetElementbyId("mw-navigation")?.Remove();
            document.GetElementbyId("footer")?.Remove();
            document.GetElementbyId("catlinks")?.Remove();
            document.GetElementbyId("toc")?.Remove();
            document.GetElementbyId("jump-to-nav")?.Remove();
            document.GetElementbyId("mw-notification-area")?.Remove();

            //kill all links
            while (document.DocumentNode.Descendants("a").FirstOrDefault() != null)
            {
                var parent = document.DocumentNode.Descendants("a").First().ParentNode;

                if (string.IsNullOrWhiteSpace(parent.InnerHtml))
                    continue;

                parent.InnerHtml = parent.InnerHtml.Replace("<a", "<span").Replace("</a", "</span");
            }

            //extra html tags
            if (filterHtmlTags != null && filterHtmlTags.Length > 0)
            {
                foreach (var filterHtmlTag in filterHtmlTags)
                {
                    document.GetElementbyId(filterHtmlTag)?.Remove();
                }
            }

            document.GetElementbyId("content")?.SetAttributeValue("style", "margin-left: 0px;");
            return document.DocumentNode.OuterHtml;
        }
        
        public string GetMasteryHtml(SkillGroupType skillGroupType)
        {
            var url = WikiConstants.SkillGroupTypeLookUp[skillGroupType];
            return GetHtml(url);
        }

        public string GetWikiUrl(SkillType skillType)
        {
            if (WikiConstants.SkillTypeLookUp.ContainsKey(skillType))
                return WikiConstants.SkillTypeLookUp[skillType];

            return string.Empty;
        }

        public string GetWikiUrl(SkillGroupType skillGroupType)
        {
            if (WikiConstants.SkillGroupTypeLookUp.ContainsKey(skillGroupType))
                return WikiConstants.SkillGroupTypeLookUp[skillGroupType];

            return string.Empty;
        }
    }
}