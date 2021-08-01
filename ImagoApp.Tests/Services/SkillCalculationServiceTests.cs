using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;
using ImagoApp.Application.Models;
using ImagoApp.Application.Services;
using ImagoApp.Shared.Enums;
using Xunit;

namespace ImagoApp.Tests.Services
{
    public class SkillCalculationServiceTests
    {
        private readonly ISkillCalculationService _skillCalculationService;

        public SkillCalculationServiceTests()
        {
            var increaseCalculationService = new IncreaseCalculationService();
            _skillCalculationService = new SkillCalculationService(increaseCalculationService);
        }

        [Theory]
        [InlineData(0, 30, 15, 0)]
        [InlineData(0, 75, 30, 0)]
        [InlineData(0, 150, 45, 0)]
        [InlineData(0, 270, 60, 0)]
        [InlineData(0, 450, 75, 0)]
        [InlineData(0, 705, 90, 0)]
        [InlineData(0, 935, 100, 0)]
        [InlineData(15, 15, 15, 0)]
        [InlineData(30, 45, 30, 0)]
        [InlineData(0, 1, 0, 1)]
        [InlineData(432, 18, 75, 0)]
        [InlineData(432, 22, 75, 4)]
        [InlineData(432, 38, 76, 3)]
        public void AddExperienceToSkill_IncreaseSuccessful(int creationEp, int experienceToAdd, int expectedIncreaseValue, int expectedExperienceLeftover)
        {
            var skill = new SkillModel(SkillModelType.Anatomie)
            {
                CreationExperience = creationEp
            };

            _skillCalculationService.AddExperience(skill, experienceToAdd);
            Assert.Equal(expectedIncreaseValue, skill.IncreaseValue);
            Assert.Equal(expectedExperienceLeftover, skill.LeftoverExperience);
        }
    }
}
