using System;
using System.Linq;
using ImagoApp.Shared.Enums;
using ImagoApp.Util;

namespace ImagoApp.Services
{
    public interface IWikiService
    {
        string GetWikiUrl(SkillModelType skillModelType);
        string GetWikiUrl(SkillGroupModelType skillGroupModelType);
        string GetTalentHtml(SkillModelType skillModelType);
        string GetMasteryHtml(SkillGroupModelType skillGroupModelType);
        string GetChangelogUrl();
    }

    public class WikiService : IWikiService
    {
        public string GetTalentHtml(SkillModelType skillModelType)
        {
            var url = WikiConstants.SkillTypeLookUp[skillModelType];
            return GetHtml(url);
        }
        
        public string GetChangelogUrl()
        {
            return WikiConstants.ChangelogUrl;
        }

        private string GetHtml(string url)
        {
            var document = WikiHelper.LoadDocumentFromUrl(url, null);
            if (document == null)
                return "";

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
            
            //kill all links
            while (document.DocumentNode.Descendants("a").FirstOrDefault() != null)
            {
                var parent = document.DocumentNode.Descendants("a").First().ParentNode;

                if (string.IsNullOrWhiteSpace(parent.InnerHtml))
                    continue;

                parent.InnerHtml = parent.InnerHtml.Replace("<a", "<span").Replace("</a", "</span");
            }
            
            document.GetElementbyId("content")?.SetAttributeValue("style", "margin-left: 0px;");
            return document.DocumentNode.OuterHtml;
        }
        
        public string GetMasteryHtml(SkillGroupModelType skillGroupModelType)
        {
            var url = WikiConstants.SkillGroupTypeLookUp[skillGroupModelType];
            return GetHtml(url);
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
    }
}