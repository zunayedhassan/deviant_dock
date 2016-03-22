using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deviant_Dock
{
    [Serializable]
    class IconSettings
    {
        public string imageLocation;
        public string iconTitle;
        public string target;
        public int iconNo;

        public IconSettings(string imageLocation, string iconTitle, string target)
        {
            setInitialValue(imageLocation, iconTitle, target);
        }

        public IconSettings(string imageLocation, string iconTitle, string target, int iconNo)
        {
            setInitialValue(imageLocation, iconTitle, target);

            this.iconNo = iconNo;
        }

        private void setInitialValue(string imageLocation, string iconTitle, string target)
        {
            this.imageLocation = imageLocation;
            this.iconTitle = iconTitle;
            this.target = target;
        }
    }
}
