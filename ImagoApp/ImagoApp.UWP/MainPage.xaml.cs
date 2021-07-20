using System;

namespace ImagoApp.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            InitializeComponent();
            LoadApplication(new ImagoApp.App(new FileService()));
        }
    }
}