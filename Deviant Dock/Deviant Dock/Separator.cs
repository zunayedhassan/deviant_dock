using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deviant_Dock
{
    class Separator
    {
        public IconSettings getSeparator()
        {
            return new IconSettings(imageLocation: "Skins/sep.png", iconTitle: string.Empty, target: string.Empty);
        }
    }
}
