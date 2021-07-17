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

                //todo need to be improved - tranlsate to middle and respect bounds
                this.TranslateTo(0,0, 250, Easing.CubicInOut);
            }
            else if (delta > 0)
            {
                if (CurrentScale >= MAX_SCALE)
                    return;

                //zoom int
                CurrentScale += 0.2;
                this.ScaleTo(CurrentScale, 250, Easing.CubicInOut);
            }
        }

        protected override SizeRequest OnMeasure(double widthConstraint, double heightConstraint)
        {
            Scale = MIN_SCALE;
            TranslationX = TranslationY = 0;
            AnchorX = AnchorY = 0;
            
            return base.OnMeasure(widthConstraint, heightConstraint);
        }

        private double _oldTranslationX;
        private double _oldTranslationY;
        private void OnPanUpdated(object sender, PanUpdatedEventArgs e)
        {
            switch (e.StatusType)
            {
                case GestureStatus.Started:
                {
                    _oldTranslationX = TranslationX;
                    _oldTranslationY = TranslationY;
                    break;
                }
                case GestureStatus.Running:
                {
                    var newX = _oldTranslationX + e.TotalX;
                    var newY = _oldTranslationY + e.TotalY;

                    Pan(newX, newY);
                    break;
                }
            }
        }

        private void Pan(double x, double y)
        {
            //prevent left
            if (x > 0)
                x = 0;

            //prevent right
            var maxTranslX = (Width * Scale) - Width;
            if (x * -1 > maxTranslX)
                x = maxTranslX * -1;

            TranslationX = x;
            OnPropertyChanged(nameof(TranslationX));

            //prevent top
            if (y > 0)
                y = 0;

            //prevent bottom
            var maxTranslY = (Height * Scale) - Height;
            if (y * -1 > maxTranslY)
                y = maxTranslY * -1;

            TranslationY = y;
            OnPropertyChanged(nameof(TranslationY));
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