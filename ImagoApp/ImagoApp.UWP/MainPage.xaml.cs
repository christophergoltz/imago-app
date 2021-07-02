namespace ImagoApp.UWP
{
    public sealed partial class MainPage
    {
        public MainPage()
        {
            this.InitializeComponent();

            LoadApplication(new ImagoApp.App());
        }
    }
}
