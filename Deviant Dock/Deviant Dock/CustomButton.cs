using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Deviant_Dock
{
    class CustomButton : Button
    {
        public CustomButton(object buttonContent, int width, int height, Thickness thickness)
        {
            this.Content = buttonContent;
            this.Width = width;
            this.Height = height;
            this.Margin = thickness;
        }

        public CustomButton(CustomImage imageIcon, int width, int height, Thickness thickness)
        {
            this.Content = imageIcon;
            this.Width = width;
            this.Height = height;
            this.Margin = thickness;
        }
    }
}
