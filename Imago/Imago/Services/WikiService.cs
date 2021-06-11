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
    }

    public class WikiService : IWikiService
    {
        public string GetTalentHtml(SkillType skillType)
        {
            var url = WikiConstants.SkillTypeLookUp[skillType];
            var web = new HtmlWeb();
            var doc = web.Load(url);
            doc.GetElementbyId("mw-page-base")?.Remove();
            doc.GetElementbyId("mw-head-base")?.Remove();
            doc.GetElementbyId("mw-navigation")?.Remove();
            doc.GetElementbyId("footer")?.Remove();
            doc.GetElementbyId("catlinks")?.Remove();
            doc.GetElementbyId("toc")?.Remove();
            //var t = doc.GetElementbyId("Beschreibung");
            //var tt = t.ParentNode;

            //while (tt.NextSibling.Name != "h2")
            //{
            //    tt.NextSibling.Remove();
            //}

            //tt.Remove();
            
            doc.GetElementbyId("content")?.SetAttributeValue("style", "margin-left: 0px;");

            return doc.DocumentNode.OuterHtml;
        }

        public string GetMasteryHtml(SkillGroupType skillGroupType)
        {
            var url = WikiConstants.SkillGroupTypeLookUp[skillGroupType];
            var web = new HtmlWeb();
            var doc = web.Load(url);
            doc.GetElementbyId("mw-page-base")?.Remove();
            doc.GetElementbyId("mw-head-base")?.Remove();
            doc.GetElementbyId("mw-navigation")?.Remove();
            doc.GetElementbyId("footer")?.Remove();
            doc.GetElementbyId("catlinks")?.Remove();
            doc.GetElementbyId("toc")?.Remove();
            doc.GetElementbyId("content")?.SetAttributeValue("style", "margin-left: 0px;");

            return doc.DocumentNode.OuterHtml;
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