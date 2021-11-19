using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImagoApp.Views.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ImageButtonView
    {
        public ImageButtonView()
        {
            InitializeComponent();
        }

        public static readonly BindableProperty TextProperty = BindableProperty.Create(
            nameof(Text), // the name of the bindable property
            typeof(string), // the bindable property type
            typeof(ImageButtonView));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set => SetValue(TextProperty, value);
        }

        public static readonly BindableProperty ImageSourceProperty = BindableProperty.Create(
            nameof(ImageSource), // the name of the bindable property
            typeof(ImageSource), // the bindable property type
            typeof(ImageButtonView));

        public ImageSource ImageSource
        {
            get => (ImageSource)GetValue(ImageSourceProperty);
            set => SetValue(ImageSourceProperty, value);
        }

        public static readonly BindableProperty CommandProperty = BindableProperty.Create(
            nameof(Command), // the name of the bindable property
            typeof(ICommand), // the bindable property type
            typeof(ImageButtonView));

        public ICommand Command
        {
            get => (ICommand)GetValue(CommandProperty);
            set => SetValue(CommandProperty, value);
        }

        public static readonly BindableProperty ImageMarginProperty = BindableProperty.Create(
            nameof(ImageMargin), // the name of the bindable property
            typeof(Thickness), // the bindable property type
            typeof(ImageButtonView),
            new Thickness(8,8,6,8), BindingMode.TwoWay);

        public Thickness ImageMargin
        {
            get => (Thickness)GetValue(ImageMarginProperty);
            set => SetValue(ImageMarginProperty, value);
        }

        public static readonly BindableProperty ImageSizeProperty = BindableProperty.Create(
            nameof(ImageSize), // the name of the bindable property
            typeof(double), // the bindable property type
            typeof(ImageButtonView),
            (double)35);

        public double ImageSize
        {
            get => (double)GetValue(ImageSizeProperty);
            set => SetValue(ImageSizeProperty, value);
        }

        public static readonly BindableProperty TextHiddenProperty = BindableProperty.Create(
            nameof(TextHidden), // the name of the bindable property
            typeof(bool), // the bindable property type
            typeof(ImageButtonView));

        public bool TextHidden
        {
            get => (bool)GetValue(TextHiddenProperty);
            set => SetValue(TextHiddenProperty, value);
        }

        public static readonly BindableProperty EnabledProperty = BindableProperty.Create(
            nameof(Enabled), // the name of the bindable property
            typeof(bool), // the bindable property type
            typeof(ImageButtonView), true);

        public bool Enabled
        {
            get => (bool)GetValue(EnabledProperty);
            set => SetValue(EnabledProperty, value);
        }

    }
}