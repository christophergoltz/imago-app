using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Text;
using SkiaSharp.Views.Forms;
using Xamarin.Forms;

namespace ImagoApp.Views.CustomControls
{
    public class CustomSKCanvas : SKCanvasView, INotifyPropertyChanged
    {
        private double _currentScale = 1;
        private double _startX;
        private double _startY;


        private const double MIN_SCALE = 1;
        private const double MAX_SCALE = 4;

        public double CurrentScale
        {
            get => _currentScale;
            private set => SetProperty(ref _currentScale, value);
        }

        public CustomSKCanvas()
        {
            var pan = new PanGestureRecognizer();
            pan.PanUpdated += OnPanUpdated;
            GestureRecognizers.Add(pan);

            var tap = new TapGestureRecognizer
            {
                NumberOfTapsRequired = 2
            };
            tap.Tapped += OnTapped;
            GestureRecognizers.Add(tap);

            Scale = MIN_SCALE;
            TranslationX = TranslationY = 0;
            AnchorX = AnchorY = 0;
        }

        public void Zoom(int delta, Point cursorLocation)
        {
            if (delta < 0)
            {
                if (CurrentScale <= MIN_SCALE)
                    return;

                //zoom out
                CurrentScale -= 0.2;
                this.ScaleTo(CurrentScale, 250, Easing.CubicInOut);
            }
            else if (delta > 0)
            {
                if (CurrentScale >= MAX_SCALE)
                    return;

                //zoom int
                CurrentScale += 0.2;
                this.ScaleTo(CurrentScale, 250, Easing.CubicInOut);
            }

            //    this.TranslateTo(cursorLocation.X, cursorLocation.Y);
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            Scale = MIN_SCALE;
            TranslationX = TranslationY = 0;
            AnchorX = AnchorY = 0;

          

            return base.OnMeasure(widthConstraint, heightConstraint);
        }

        private void OnTapped(object sender, EventArgs e)
        {

            Debug.WriteLine("OnTapped");
            if (Scale > MIN_SCALE)
            {
                this.ScaleTo(MIN_SCALE, 250, Easing.CubicInOut);
                this.TranslateTo(0, 0, 250, Easing.CubicInOut);
            }
            else
            {
                AnchorX = AnchorY = 0.5; //TODO tapped position
                this.ScaleTo(MAX_SCALE, 250, Easing.CubicInOut);
            }
        }


        public double StartX
        {
            get => _startX;
            private set => SetProperty(ref _startX, value);
        }

        public double StartY
        {
            get => _startY;
            private set => SetProperty(ref _startY, value);
        }

        public double AAnchorX => AnchorX;
        public double AAnchorY => AnchorY;

        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                    StartX = (1 - AnchorX) * Width;
                    StartY = (1 - AnchorY) * Height;
                    break;
                case GestureStatus.Running:
                {
                    //normal behavior at Scale 2
                    var transformMargin = CurrentScale - 1;

                    //x
                    var clampX = Clamp(1 - (StartX + e.TotalX) / Width, 0, 1);
                    double newAnchorX = transformMargin == 0
                        ? clampX
                        : clampX / transformMargin;
                    AnchorX = newAnchorX;
                    OnPropertyChanged(nameof(AAnchorX));

                    //y
                    var clampY = Clamp(1 - (StartY + e.TotalY) / Height, 0, 1);
                    double newAnchorY = transformMargin == 0
                        ? clampY
                        : clampY / transformMargin;
                    AnchorY = newAnchorY;
                    OnPropertyChanged(nameof(AAnchorY));
                    break;
                }
            }
        }

        private T Clamp<T>(T value, T minimum, T maximum) where T : IComparable
        {
            if (value.CompareTo(minimum) < 0)
                return minimum;
            else if (value.CompareTo(maximum) > 0)
                return maximum;
            else
                return value;
        }

        #region INotifyPropertyChanged

        protected bool SetProperty<T>(ref T backingStore, T value,
            [CallerMemberName] string propertyName = "",
            Action onChanged = null)
        {
            if (EqualityComparer<T>.Default.Equals(backingStore, value))
                return false;

            backingStore = value;
            onChanged?.Invoke();
            OnPropertyChanged(propertyName);
            return true;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = "")
        {
            var changed = PropertyChanged;
            if (changed == null)
                return;

            changed.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion
    }
}