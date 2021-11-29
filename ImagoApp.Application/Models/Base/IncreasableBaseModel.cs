using System;
using System.Diagnostics;
using ImagoApp.Application.Services;
using ImagoApp.Shared.Enums;

namespace ImagoApp.Application.Models.Base
{
    public abstract class IncreasableBaseModel : ModifiableBaseModel
    {
        private readonly IncreaseType _increaseType;
        public IncreasableBaseModel(IncreaseType increaseType)
        {
            _increaseType = increaseType;
        }

        private int _experienceValue;

        public int ExperienceValue
        {
            get => _experienceValue;
            set
            {
                SetProperty(ref _experienceValue, value);
                OnPropertyChanged(nameof(IncreaseValueCache));
                OnPropertyChanged(nameof(ExperienceForNextIncreasedRequiredCache));
                OnPropertyChanged(nameof(LeftoverExperienceCache));
            }
        }

        public int IncreaseValueCache => GetIncreaseInfo().IncreaseLevel;
        public int ExperienceForNextIncreasedRequiredCache => GetIncreaseInfo().ExperienceForNextIncrease;
        public int LeftoverExperienceCache => GetIncreaseInfo().LeftoverExperience;

        private static long _ell;

        private IncreaseInfoModel _oldIncreaseInfo;
        private IncreaseInfoModel GetIncreaseInfo()
        {
            int totalExperience;
            switch (_increaseType)
            {
                case IncreaseType.SkillGroup:
                    totalExperience = ExperienceValue;
                    break;
                case IncreaseType.Skill:
                    totalExperience = ExperienceValue + ((SkillModel)this).CreationExperience;
                    break;
                case IncreaseType.Attribute:
                    totalExperience = ExperienceValue + ((AttributeModel)this).CreationExperience + ((AttributeModel)this).ExperienceBySkillGroup;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(IncreaseType));
            }

            if (_oldIncreaseInfo != null && _oldIncreaseInfo.TotalExperience == totalExperience)
                return _oldIncreaseInfo;

            var st = Stopwatch.StartNew();

            //ex got changed, recalc
            var info = IncreaseCalculationService.GetIncreaseInfo(_increaseType, totalExperience);
            _oldIncreaseInfo = info;
            st.Stop();
            _ell += st.ElapsedTicks;

            Debug.WriteLine($"### GetIncreaseInfo {ToString()}, Value: {info.IncreaseLevel}, T: {st.ElapsedTicks}, Time:{_ell} ticks");

            return info;
        }
    }
}