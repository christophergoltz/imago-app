using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ImagoApp.Views.CustomControls;
using SkiaSharp;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImagoApp.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WorldMapPage : ContentPage
    {
        public WorldMapPage()
        {
            InitializeComponent();
        }

        public void Zoom(int delta, Point cursorLocation)
        {
            DrawingSKCanvas.Zoom(delta, cursorLocation);
        }
        
        private void SKCanvasView_OnPaintSurface(object sender, SKPaintSurfaceEventArgs e)
        {
            SKImageInfo info = e.Info;
            SKSurface surface = e.Surface;
            SKCanvas canvas = surface.Canvas;

            canvas.Clear();

            SKPaint paint = new SKPaint
            {
                Style = SKPaintStyle.Stroke,
                Color = Color.Red.ToSKColor(),
                StrokeWidth = 50
            };
            canvas.DrawRect((info.Width -600) / 2, (info.Height -600) / 2, 1700,1200, paint);

        }
    }
}