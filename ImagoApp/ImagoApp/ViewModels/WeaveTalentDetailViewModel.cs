using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Input;
using ImagoApp.Application;
using ImagoApp.Application.Models;
using ImagoApp.Application.Services;
using ImagoApp.Shared.Enums;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class WeaveTalentDetailViewModel : BindableBase
    {
        private WeaveTalentModel _weaveTalent;
        private List<SkillModel> _skills;
        private List<MasteryListItemViewModel> _masteries;
        private SkillModel _selectedSkillModel;
        private int _concentrationPerAction;
        private int _concentrationQuantity;
        private int _modification;
        private int _finalValue;
        private bool _allDifficultyRemoved;
        public ICommand CloseCommand { get; set; }

        public event EventHandler CloseRequested;

        public event EventHandler<SkillModelType> OpenSkillPageRequested;

        public WeaveTalentDetailViewModel(WeaveTalentModel weaveTalent, List<SkillModel> skills)
        {
            WeaveTalent = weaveTalent;
            Skills = skills;
         

            CloseCommand = new Command(() =>
            {
                CloseRequested?.Invoke(this, EventArgs.Empty);
            });
       
        }
        
        private ICommand _openSkillCommand;

        public ICommand OpenSkillCommand => _openSkillCommand ?? (_openSkillCommand = new Command<SkillModelType>(skill =>
        {
            OpenSkillPageRequested?.Invoke(this, skill);
        }));
        
        public int FinalValue
        {
            get => _finalValue;
            set => SetProperty(ref _finalValue, value);
        }
        
        public List<MasteryListItemViewModel> Masteries
        {
            get => _masteries;
            set => SetProperty(ref _masteries, value);
        }

        public List<SkillModel> Skills
        {
            get => _skills;
            private set => _skills = value;
        }

        public WeaveTalentModel WeaveTalent
        {
            get => _weaveTalent;
            private set => _weaveTalent = value;
        }
    }
}
