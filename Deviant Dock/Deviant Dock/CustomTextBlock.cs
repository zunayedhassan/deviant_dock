using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Deviant_Dock
{
    class CustomTextBlock : TextBlock
    {
        public CustomTextBlock(string text)
        {
            this.Text = text;
            this.TextAlignment = TextAlignment.Center;
        }
    }
}
