using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using ImagoApp.Application.Services;
using ImagoApp.ViewModels;
using ImagoApp.Views;
using Xamarin.Forms;

namespace ImagoApp.Manager
{
    public class ErrorManager
    {
        private readonly IErrorService _errorService;

        public ErrorManager(IErrorService errorService)
        {
            _errorService = errorService;
        }

        public void TrackExceptionSilent(Exception exception, string affectedCharacter = null, bool includeCharacterDatabase = false,
            Dictionary<string, string> customProperites = null)
        {
            Debug.WriteLine(exception);

            App.SaveCurrentCharacter();

            var stackTrace = Environment.StackTrace;

            if (customProperites == null)
                customProperites = new Dictionary<string, string>();

            if (!string.IsNullOrWhiteSpace(affectedCharacter))
                customProperites.Add("Affected Character",affectedCharacter);

            _errorService.TrackException(exception, customProperites, includeCharacterDatabase, null, stackTrace);
        }

        public void TrackException(Exception exception, string affectedCharacter = null, Dictionary<string, string> customProperites = null)
        {
            Debug.WriteLine(exception);

            App.SaveCurrentCharacter();

            var stackTrace = Environment.StackTrace;

            //show ui
            var vm = new ErrorPageViewModel(affectedCharacter);
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

                if (!string.IsNullOrWhiteSpace(args.AffectedCharacter))
                    customProperites.Add("Affected Character", args.AffectedCharacter);
                
                _errorService.TrackException(exception, customProperites, args.IncludeDatabase, args.Description, stackTrace);

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