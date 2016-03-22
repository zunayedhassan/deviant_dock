using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;

namespace Deviant_Dock
{
    class CustomMenuItem : MenuItem
    {
        public CustomMenuItem(string text, ref ContextMenu contextMenu)
        {
            this.Header = text;
            contextMenu.Items.Add(this);
        }

        public CustomMenuItem(string text, ref CustomMenuItem menuItem)
        {
            this.Header = text;
            menuItem.Items.Add(this);
        }

        public CustomMenuItem(string text)
        {
            this.Header = text;
        }
    }
}
