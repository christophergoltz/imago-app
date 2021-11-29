using System;
using System.Diagnostics;
using ImagoApp.Application;
using ImagoApp.Application.Models;
using ImagoApp.Shared.Enums;

namespace ImagoApp.ViewModels
{
    public class HandicapListViewItemViewModel : BindableBase
    {
        private bool _isChecked;
        private string _imageSource;
        private string _text;
        private DerivedAttributeModel _value;
        public event EventHandler HandicapValueChanged;
        
        public bool IsChecked
        {
            get => _isChecked;
            set
            {
                //todo why is this set twice when radio button value changed?
                SetProperty(ref _isChecked, value);
                HandicapValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }
        
        public string ImageSource
        {
            get => _imageSource;
            set => SetProperty(ref _imageSource ,value);
        }

        public string Text
        {
            get => _text;
            set => SetProperty(ref _text, value);
        }

        public DerivedAttributeModel Value
        {
            get => _value;
            set => SetProperty(ref _value ,value);
        }

        public HandicapListViewItemViewModel(DerivedAttributeModel value, bool isChecked, string imageSource, string text)
        {
            Value = value;
            IsChecked = isChecked;
            ImageSource = imageSource;
            Text = text;
        }
    }
}