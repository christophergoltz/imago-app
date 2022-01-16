using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Shapes;
using Xamarin.Forms.Xaml;

namespace ImagoApp.Views.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TallyMarker : ContentView
    {
        public TallyMarker()
        {
            InitializeComponent();
        }

        public int MaximumValue
        {
            get { return (int)GetValue(MaximumValueProperty); }
            set { SetValue(MaximumValueProperty, value); }
        }
        
        public static readonly BindableProperty MaximumValueProperty =
            BindableProperty.Create(nameof(MaximumValue),
                typeof(int),
                typeof(TallyMarker),
                0,
                BindingMode.TwoWay,
                null,
                (bindable, value, newValue) => ((TallyMarker)bindable).Redraw());

        public int CurrentValue
        {
            get { return (int)GetValue(CurrentValueProperty); }
            set { SetValue(CurrentValueProperty, value); }
        }
        
        public static readonly BindableProperty CurrentValueProperty =
            BindableProperty.Create(nameof(MaximumValue),
                typeof(int),
                typeof(TallyMarker),
                0,
                BindingMode.TwoWay,
                null,
                (bindable, value, newValue) => ((TallyMarker)bindable).Redraw());

        private readonly Random _rnd = new Random();

        private readonly double[] _markRotations = {
            -3,
            -2, -2,
            -1, -1,
            1, 1,
            2, 2,
            3
        };

        private void Redraw()
        {
            //clean up old
            OuterContainer.Children.Clear();

            var currentValue = CurrentValue;
            var openValue = MaximumValue - currentValue;
            
            var strichStyle = new Style(typeof(Path))
            {
                BaseResourceKey = "Strich",
            };

            //queue all marks
            var marks = new Queue<Path>();
            for (var i = 0; i < currentValue; i++)
            {
                var p = new Path()
                {
                    Fill = Brush.Black,
                    Style = strichStyle,
                    Aspect = Stretch.Uniform,
                    HeightRequest = 35,
                    Rotation = _markRotations[_rnd.Next(0, _markRotations.Length)]
                };

                marks.Enqueue(p);
            }

            for (var i = 0; i < openValue; i++)
            {
                var p = new Path()
                {
                    Fill = Brush.Black,
                    Style = strichStyle,
                    Aspect = Stretch.Uniform,
                    HeightRequest = 35,
                    Opacity = 0.3,
                    Rotation = _markRotations[_rnd.Next(0, _markRotations.Length)]
                };

                marks.Enqueue(p);
            }

            //put marks into groups
            var groupCounter = 0;
            StackLayout group2 = null;
            while (marks.Any())
            {
                var path = marks.Dequeue();
                groupCounter++;

                if (groupCounter == 1)
                {
                    group2 = new StackLayout()
                    {
                        Orientation = StackOrientation.Horizontal
                    };
                }

                group2.Children.Add(path);

                if (groupCounter == 5)
                {
                    //last one, rotate
                    path.Rotation = 70;
                    path.HeightRequest = 50;
                    path.Margin = new Thickness(-25, -7, 25, -7);

                    OuterContainer.Children.Add(group2);
                    groupCounter = 0;
                }
            }

            if(group2 != null)
            {
                if (!OuterContainer.Children.Contains(group2))
                {
                    OuterContainer.Children.Add(group2);
                }
            }
        }
    }
}