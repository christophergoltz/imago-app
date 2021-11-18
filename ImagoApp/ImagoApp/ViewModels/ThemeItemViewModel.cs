using System;
using System.Collections.Generic;
using System.Text;
using ImagoApp.Views.CustomControls;
using Xamarin.Forms;

namespace ImagoApp.ViewModels
{

    public class ThemeItemViewModel
    {
        public ApplicationTheme Theme { get; set; }
        public ResourceDictionary ResourceDictionary { get; set; }
        public Color PrimaryColor { get; set; }
        public Color SecondaryColor { get; set; }
    }

}
