using System.ComponentModel;
using ImagoApp.UWP;
using Xamarin.Forms;
using Xamarin.Forms.Platform.UWP;

[assembly: ExportRenderer(typeof(Entry), typeof(ExtendedEntryRenderer))]
namespace ImagoApp.UWP
{
    public class ExtendedEntryRenderer : EntryRenderer
    {
        protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            base.OnElementPropertyChanged(sender, e);
            if (e.PropertyName == nameof(Entry.IsFocused)
                && Control != null && Control.FocusState != Windows.UI.Xaml.FocusState.Unfocused)
                Control.SelectAll();
        }
    }
}