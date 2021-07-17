using System;
using System.Diagnostics;
using Windows.Foundation;
using Windows.UI.Core.Preview;
using Windows.UI.Xaml.Input;
using Acr.UserDialogs;
using Microsoft.AppCenter.Crashes;

namespace ImagoApp.UWP
{
    public sealed partial class MainPage
    {
        private ImagoApp.App _app;

        public MainPage()
        {
            InitializeComponent();
            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += OnCloseRequest;
            _app = new ImagoApp.App(new FileService());
            LoadApplication(_app);
        }
        
        private void OnCloseRequest(object sender, SystemNavigationCloseRequestedPreviewEventArgs e)
        {
            var saved = ImagoApp.App.SaveCurrentCharacter();
            if (!saved)
            {
                e.Handled = true;

                Crashes.TrackError(new InvalidOperationException("Character couldnt be saved on app quit"));

                UserDialogs.Instance.Confirm(new ConfirmConfig
                {
                    Message = $"{Environment.NewLine}Wie soll fortgefahren werden?" +
                              $"{Environment.NewLine}{Environment.NewLine}      Abbrechen: Die App bleibt geöffnet und das Speichern kann erneut versucht werden" +
                              $"{Environment.NewLine}{Environment.NewLine}      App beenden: Die letzten Änderungen am Charakter werden nicht gespeichert und die App wird beendet",
                    Title = "Fehler, der Charakter konnte nicht gespeichert werden",
                    OkText = "Abbrechen",
                    CancelText = "App beenden",
                    OnAction = confirmResult =>
                    {
                        if (!confirmResult)
                        {
                            //user confirmed to close app anyway
                            Windows.UI.Xaml.Application.Current.Exit();
                        }
                    }
                });
            }
        }

        private void MainPage_OnPointerWheelChanged(object sender, PointerRoutedEventArgs e)
        {
            Windows.UI.Input.PointerPoint pp = e.GetCurrentPoint(sender as MainPage);
            int wheelDelta = pp.Properties.MouseWheelDelta;

            Point cursorLocation = pp.Position;
            _app.Zoom(wheelDelta, new Xamarin.Forms.Point(cursorLocation.X, cursorLocation.Y));
        }
    }
}