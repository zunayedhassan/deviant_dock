using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Deviant_Dock
{
    class CustomLabel : Label
    {
        public CustomLabel(string text, Thickness thickness)
        {
            this.Content = text;
            this.Margin = thickness;
        }
    }
}
