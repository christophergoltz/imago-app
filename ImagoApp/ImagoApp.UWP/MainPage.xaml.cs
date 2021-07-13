using System;
using Windows.UI.Core.Preview;
using Acr.UserDialogs;

namespace ImagoApp.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            SystemNavigationManagerPreview.GetForCurrentView().CloseRequested += OnCloseRequest;
            LoadApplication(new ImagoApp.App());
        }
        
        private void OnCloseRequest(object sender, SystemNavigationCloseRequestedPreviewEventArgs e)
        {
            var saved = ImagoApp.App.SaveCurrentCharacter();
            if (!saved)
            {
                e.Handled = true;
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
    }
}