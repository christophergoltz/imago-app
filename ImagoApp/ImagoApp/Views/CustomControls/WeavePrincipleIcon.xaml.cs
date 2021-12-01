using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace ImagoApp.Views.CustomControls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class WeavePrincipleIcon
    {
        public WeavePrincipleIcon()
        {
            InitializeComponent();
        }
        
        public static readonly BindableProperty NameProperty = BindableProperty.Create(
            nameof(Name), // the name of the bindable property
            typeof(string), // the bindable property type
            typeof(WeavePrincipleIcon));

        public string Name
        {
            get => (string)GetValue(NameProperty);
            set => SetValue(NameProperty, value);
        }
    }
}