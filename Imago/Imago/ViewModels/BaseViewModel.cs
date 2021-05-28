using Imago.Models;
using Imago.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Imago.Util;
using Xamarin.Forms;

namespace Imago.ViewModels
{
    public class BaseViewModel : BindableBase
    {
        bool _isBusy;
        public bool IsBusy
        {
            get => _isBusy;
            set => SetProperty(ref _isBusy, value);
        }

        string _title = string.Empty;
        public string Title
        {
            get => _title;
            set => SetProperty(ref _title, value);
        }

    }
}
