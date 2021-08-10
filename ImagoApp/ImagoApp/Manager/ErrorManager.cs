using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ImagoApp.Application.Services;
using ImagoApp.Infrastructure.Database;
using ImagoApp.ViewModels;
using ImagoApp.Views;
using Xamarin.Forms;

namespace ImagoApp.Manager
{
    public class ErrorManager
    {
        private readonly IErrorService _errorService;
        private readonly ICharacterDatabaseConnection _characterDatabaseConnection;
        private readonly ICharacterProvider _characterProvider;
        private readonly ICharacterService _characterService;

        public ErrorManager(IErrorService errorService, ICharacterDatabaseConnection characterDatabaseConnection, ICharacterProvider characterProvider, ICharacterService characterService)
        {
            _errorService = errorService;
            _characterDatabaseConnection = characterDatabaseConnection;
            _characterProvider = characterProvider;
            _characterService = characterService;
        }

        public void TrackExceptionSilent(Exception exception, Dictionary<string, string> customProperites = null)
        {
            Debug.WriteLine(exception);

            App.SaveCurrentCharacter();

            var stackTrace = Environment.StackTrace;

            if (customProperites == null)
                customProperites = new Dictionary<string, string>();

            _errorService.TrackException(exception, customProperites, null, stackTrace);
        }

        public void TrackException(Exception exception, string affectedCharacter = null, Dictionary<string, string> customProperites = null)
        {
            Debug.WriteLine(exception);

            App.SaveCurrentCharacter();

            var stackTrace = Environment.StackTrace;

            //show ui
            var vm = new ErrorPageViewModel(affectedCharacter, _characterService);
            vm.OnCancelled += (sender, args) =>
            {
                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Xamarin.Forms.Application.Current.MainPage.Navigation.PopModalAsync();
                });
            };
            vm.OnErrorReportSend += (sender, args) =>
            {
                if (customProperites == null)
                    customProperites = new Dictionary<string, string>();
                
                var selectedAttachments = args.Attachments
                    .Where(database => database.IsSelected)
                    .Select(database => database.FilePath)
                    .ToList();

                if (selectedAttachments.Any())
                {
                    _errorService.TrackException(exception, customProperites, args.Description, stackTrace, selectedAttachments.ToArray());
                }
                else
                {
                    _errorService.TrackException(exception, customProperites, args.Description, stackTrace);
                }

                Device.BeginInvokeOnMainThread(async () =>
                {
                    await Xamarin.Forms.Application.Current.MainPage.Navigation.PopModalAsync();
                });
            };

            //show errorpage on ui
            var page = new ErrorPage(vm);
            Device.BeginInvokeOnMainThread(async () =>
            {
                await Xamarin.Forms.Application.Current.MainPage.Navigation.PushModalAsync(page);
            });
        }
    }
}