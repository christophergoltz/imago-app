using System;
using Windows.Foundation;
using Windows.Graphics.Display;
using Windows.UI.ViewManagement;

namespace ImagoApp.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            MaximizeWindowOnLoad();
            MaximizeWindowOnLoad2();

            InitializeComponent();
            LoadApplication(new ImagoApp.App(new LocalFileService()));
        }

        private static void MaximizeWindowOnLoad()
        {
            try
            {
                // Get how big the window can be in epx.
                var bounds = ApplicationView.GetForCurrentView().VisibleBounds;

                ApplicationView.PreferredLaunchViewSize = new Size(bounds.Width, bounds.Height);
                ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            }
            catch (Exception)
            {
                //ignored
            }
        }

        private static void MaximizeWindowOnLoad2()
        {
            try
            {
                var view = DisplayInformation.GetForCurrentView();

                // Get the screen resolution (APIs available from 14393 onward).
                var resolution = new Size(view.ScreenWidthInRawPixels, view.ScreenHeightInRawPixels);

                // Calculate the screen size in effective pixels. 
                // Note the height of the Windows Taskbar is ignored here since the app will only be given the maxium available size.
                var scale = view.ResolutionScale == ResolutionScale.Invalid ? 1 : view.RawPixelsPerViewPixel;
                var bounds = new Size(resolution.Width / scale, resolution.Height / scale);

                ApplicationView.PreferredLaunchViewSize = new Size(bounds.Width, bounds.Height);
                ApplicationView.PreferredLaunchWindowingMode = ApplicationViewWindowingMode.PreferredLaunchViewSize;
            }
            catch (Exception)
            {
                //ignored
            }
        }
    }
}