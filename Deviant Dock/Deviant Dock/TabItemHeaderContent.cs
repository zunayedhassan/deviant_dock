using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;

namespace Deviant_Dock
{
    class TabItemHeaderContent : DockPanel
    {
        public TabItemHeaderContent(string text, CustomImage icon)
        {
            DockPanel.SetDock(icon, Dock.Top);

            TextBlock title = new TextBlock();
            title.HorizontalAlignment = HorizontalAlignment.Center;
            title.Text = text;

            DockPanel.SetDock(title, Dock.Bottom);

            this.Children.Add(icon);
            this.Children.Add(title);
        }
    }
}
