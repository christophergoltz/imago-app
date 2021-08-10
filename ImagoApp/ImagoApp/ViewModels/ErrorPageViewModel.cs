using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ImagoApp.Application;
using ImagoApp.Application.Services;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{
    public class ErrorDatabase
    {
        public string Name { get; set; }
        public string FilePath { get; set; }
        public bool IsSelected { get; set; }
    }

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
        private List<ErrorDatabase> _attachments;

        public ICommand SendCommand => _sendCommand ?? (_sendCommand = new Command(viewmodel =>
        {
            OnErrorReportSend?.Invoke(this, this);
        }));

        public List<ErrorDatabase> Attachments
        {
            get => _attachments;
            set => SetProperty(ref _attachments, value);
        }

        public ErrorPageViewModel(string affectedCharacter, ICharacterService characterService)
        {
            Attachments = characterService.GetAllQuick().Select(characterPreview => new ErrorDatabase
            {
                FilePath = characterPreview.FilePath,
                IsSelected = false,
                Name = characterPreview.Name
            }).ToList();

            if (!string.IsNullOrWhiteSpace(affectedCharacter))
            {
                var selected = Attachments.FirstOrDefault(database => database.Name.Equals(affectedCharacter));
                if (selected != null)
                    selected.IsSelected = true;
            }
        }

        public string Description { get; set; }
    }
}
