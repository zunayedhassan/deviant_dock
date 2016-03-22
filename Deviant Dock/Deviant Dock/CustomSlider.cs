using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Deviant_Dock
{
    class CustomSlider : Slider
    {
        public CustomSlider(int width, int startValue, int endValue, Thickness thickness)
        {
            this.Width = width;
            this.Minimum = startValue;
            this.Maximum = endValue;
            this.Margin = thickness;
        }
    }
}
