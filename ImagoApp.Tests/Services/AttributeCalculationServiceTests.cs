using System;
using System.Collections.Generic;
using System.Text;
using ImagoApp.Application.Models;
using ImagoApp.Application.Services;
using ImagoApp.Shared.Enums;
using Xunit;

namespace ImagoApp.Tests.Services
{
    public class AttributeCalculationServiceTests
    {
        private readonly IAttributeCalculationService _attributeCalculationService;

        public AttributeCalculationServiceTests()
        {
            var increaseCalculationService = new IncreaseCalculationService();
            var skillCalculationService = new SkillCalculationService(increaseCalculationService);
            var skillGroupCalculationService =
                new SkillGroupCalculationService(skillCalculationService, increaseCalculationService);
            _attributeCalculationService =
                new AttributeCalculationService(increaseCalculationService, skillGroupCalculationService);
        }

        [Theory]
        [InlineData(30, 30, 30, 0)]
        [InlineData(0, 90, 40, 0)]
        [InlineData(100, 80, 55, 0)]
        [InlineData(100, 85, 55, 5)]
        [InlineData(300, 42, 70, 2)]
        [InlineData(510, 3, 80, 3)]
        public void AddExperienceToAttribute_IncreaseSuccessful(int creationEp, int experienceToAdd,
            int expectedIncreaseValue, int expectedExperienceLeftover)
        {
            var skill = new SkillModel(SkillModelType.Bewusstsein);

            var skillGroup = new SkillGroupModel(SkillGroupModelType.Fernkampf)
            {
                ExperienceValue = creationEp,
                Skills = new List<SkillModel>()
                {
                    skill
                }
            };

            var attribute = new AttributeModel(AttributeType.Intelligenz)
            {
                CreationExperience = creationEp
            };

            var attributes = new List<AttributeModel>() {attribute};
            var skillGroups = new List<SkillGroupModel>() {skillGroup};

            _attributeCalculationService.AddSkillGroupExperience(attribute, experienceToAdd, attributes, skillGroups);
            Assert.Equal(expectedIncreaseValue, attribute.IncreaseValue);
            Assert.Equal(expectedExperienceLeftover, attribute.LeftoverExperience);
        }
    }
}