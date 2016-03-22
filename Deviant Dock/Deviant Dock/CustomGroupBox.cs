using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Deviant_Dock
{
    class CustomGroupBox : GroupBox
    {
        public Canvas groupBoxCanvas = new Canvas();

        public CustomGroupBox(string title, int width, int height, Thickness thickness, ref Canvas baseCanvas)
        {
            this.Header = title;
            this.Width = width;
            this.Height = height;
            this.Margin = thickness;
            this.Content = groupBoxCanvas;

            baseCanvas.Children.Add(this);
        }
    }
}
