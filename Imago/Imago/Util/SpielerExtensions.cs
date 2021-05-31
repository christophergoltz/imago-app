using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Imago.Models;
using Imago.Models.Enum;

namespace Imago.Util
{
    public static class SpielerExtensions
    {
        public static List<SkillGroup> GetAllSkillGroupsByAttribute(this Character character, AttributeType attributeType)
        {
            var result = new List<SkillGroup>();
            if (character.Bewegung.SkillSource.Any(_ => _ == attributeType))
                result.Add(character.Bewegung);
            if (character.Heimlichkeit.SkillSource.Any(_ => _ == attributeType))
                result.Add(character.Heimlichkeit);
            if (character.Nahkampf.SkillSource.Any(_ => _ == attributeType))
                result.Add(character.Nahkampf);
            if (character.Fernkampf.SkillSource.Any(_ => _ == attributeType))
                result.Add(character.Fernkampf);
            if (character.Handwerk.SkillSource.Any(_ => _ == attributeType))
                result.Add(character.Handwerk);
            if (character.Wissenschaft.SkillSource.Any(_ => _ == attributeType))
                result.Add(character.Wissenschaft);
            if (character.Webkunst.SkillSource.Any(_ => _ == attributeType))
                result.Add(character.Webkunst);
            if (character.Soziales.SkillSource.Any(_ => _ == attributeType))
                result.Add(character.Soziales);
            return result;
        }
    }
}
