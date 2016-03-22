using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Media;

namespace Deviant_Dock
{
    [Serializable]
    class PrimaryDockySettings
    {
        /* General */
        public int totalIcon = 8,                       // Default: 9
                   totalSeparator = 2;                  // Default: 2

        /* Icon */
        public string hoaverEffect = "Zoom";            // Zoom (default), Swing, Fade, Rotate
        public string clickEffect = "Swing";            // Zoom, Swing (default), Fade, Rotate
 
        /* Position */
        public string screenPosition = "Top";           // Top (default), Bottom, Left, Right
        public string layering = "Normal";              // Normal (default), Topmost
        public int centering = 0;                       // -100 to 100 % (Default: 0%)
        public int edgeOffset = 10;                     // -15px to 128px (Default: 10px)

        /* Style */
        public string theme = "VistaBlack";             // VistaBlack (default)
        public bool showIconLabel = true;               // true (default), false
    }
}
