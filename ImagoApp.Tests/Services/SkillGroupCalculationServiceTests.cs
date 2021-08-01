using System;
using System.Collections.Generic;
using System.Text;
using ImagoApp.Application.Models;
using ImagoApp.Application.Services;
using ImagoApp.Shared.Enums;
using Xunit;

namespace ImagoApp.Tests.Services
{
    public class SkillGroupCalculationServiceTests
    {
        private readonly ISkillGroupCalculationService _skillGroupCalculationService;

        public SkillGroupCalculationServiceTests()
        {
            var increaseCalculationService = new IncreaseCalculationService();
            var skillCalculationService = new SkillCalculationService(increaseCalculationService);
            _skillGroupCalculationService = new SkillGroupCalculationService(skillCalculationService,increaseCalculationService);
        }

        [Theory]
        [InlineData(2, 23, 5, 0)]
        [InlineData(0, 65, 10, 0)]
        [InlineData(20, 47, 10, 2)]
        [InlineData(890, 10, 40, 0)]
        public void AddExperienceToSkillGroup_IncreaseSuccessful(int experienceValue, int experienceToAdd, int expectedIncreaseValue, int expectedExperienceLeftover)
        {
            var skill = new SkillModel(SkillModelType.Bewusstsein);

            var skillGroup = new SkillGroupModel(SkillGroupModelType.Fernkampf)
            {
                ExperienceValue = experienceValue,
                Skills = new List<SkillModel>()
                {
                    skill
                }
            };
            
            _skillGroupCalculationService.AddExperience(skillGroup, experienceToAdd);
            Assert.Equal(expectedIncreaseValue, skillGroup.IncreaseValueCache);
            Assert.Equal(expectedExperienceLeftover, skillGroup.LeftoverExperienceCache);
        }
    }
}
