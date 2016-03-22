using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Deviant_Dock
{
    class CustomComboBox : ComboBox
    {
        public CustomComboBox(string[] items, int width,  Thickness thickness)
        {
            foreach (var item in items)
                this.Items.Add(item);

            this.Width = width;
            this.Margin = thickness;
        }
    }
}
