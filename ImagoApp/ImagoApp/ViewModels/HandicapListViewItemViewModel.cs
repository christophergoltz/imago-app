using System;
using ImagoApp.Application;
using ImagoApp.Shared.Enums;

namespace ImagoApp.ViewModels
{
    public class HandicapListViewItemViewModel : BindableBase
    {
        private DerivedAttributeType _type;
        private bool _isChecked;
        private int? _handiCapValue;
        private string _imageSource;
        private string _text;
        public event EventHandler HandicapValueChanged;

        public DerivedAttributeType Type
        {
            get => _type;
            set
            {
                SetProperty(ref _type, value);
                HandicapValueChanged?.Invoke(this, EventArgs.Empty);
            }
        }

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

        public int? HandiCapValue
        {
            get => _handiCapValue;
            set
            {
                SetProperty(ref _handiCapValue, value);
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

        public HandicapListViewItemViewModel(DerivedAttributeType type, bool isChecked, int? handiCapValue, string imageSource, string text)
        {
            Type = type;
            IsChecked = isChecked;
            HandiCapValue = handiCapValue;
            ImageSource = imageSource;
            Text = text;
        }
    }
}