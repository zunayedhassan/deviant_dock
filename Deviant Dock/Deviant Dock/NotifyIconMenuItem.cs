using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Deviant_Dock
{
    class NotifyIconMenuItem : MenuItem
    {
        public NotifyIconMenuItem(string text, ref ContextMenu contextMenu)
        {
            this.Text = text;
            contextMenu.MenuItems.Add(this);
        }

        public NotifyIconMenuItem(string text, ref NotifyIconMenuItem menuItem)
        {
            this.Text = text;
            menuItem.MenuItems.Add(this);
        }
    }
}
