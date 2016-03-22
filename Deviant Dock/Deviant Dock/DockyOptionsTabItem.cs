using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Controls;
using System.Windows.Media;

namespace Deviant_Dock
{
    class DockyOptionsTabItem : TabItem
    {
        private int STANDARD_ICON_DIMENSION = 64;

        public DockyOptionsTabItem(string title, string iconLocation, ref TabControl tabControl)
        {
            this.Header = new TabItemHeaderContent(text: title, icon: new CustomImage(imageName: iconLocation, width: STANDARD_ICON_DIMENSION / 2, height: STANDARD_ICON_DIMENSION/2));
            this.Width = STANDARD_ICON_DIMENSION;
            tabControl.Items.Add(this);
        }
    }
}
