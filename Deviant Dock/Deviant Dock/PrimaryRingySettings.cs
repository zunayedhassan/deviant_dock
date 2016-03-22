using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Deviant_Dock
{
    [Serializable]
    class PrimaryRingySettings
    {
        public string theme = "Black";
        public string hoaverEffect = "Rotate";        // Zoom, Fade, Rotate (default)
        public string logoImageLocation = "Icons/win_logo.png";
        public bool showIconLabel = false;
    }
}
