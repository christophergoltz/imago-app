using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using ImagoApp.Application;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class ErrorPageViewModel : BindableBase
    {
        public event EventHandler OnCancelled;
        public event EventHandler<ErrorPageViewModel> OnErrorReportSend;

        private ICommand _cancelCommand;
        public ICommand CancelCommand => _cancelCommand ?? (_cancelCommand = new Command(() =>
        {
            OnCancelled?.Invoke(this, EventArgs.Empty);
        }));

        private ICommand _sendCommand;
        private bool _includeDatabase;

        public ICommand SendCommand => _sendCommand ?? (_sendCommand = new Command(viewmodel =>
        {
            OnErrorReportSend?.Invoke(this, this);
        }));

        public ErrorPageViewModel(string affectedCharacter)
        {
            AffectedCharacter = affectedCharacter;
            IncludeDatabase = true;
        }

        public bool IncludeDatabase
        {
            get => _includeDatabase;
            set => SetProperty(ref _includeDatabase, value);
        }

        public string AffectedCharacter  { get; set; }
        public string Description  { get; set; }
    }
}
