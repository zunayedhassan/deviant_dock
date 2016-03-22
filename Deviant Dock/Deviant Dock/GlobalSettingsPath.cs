using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Deviant_Dock
{
    static class GlobalSettingsPath
    {
        public static string path;

        static GlobalSettingsPath()
        {
            // NOTE: This class is only required for creatings setup file. For some reason, Microsoft Windows won't allow to read/write our settings file within 'C:\Program Files' directory. So, as a temporary solution, we use 'C:\Users\<your_account_name>\App Data\Roaming\Deviant Dock\Settings' directory for reading/writings settings file. But, when we use this software as 'portable' version, we use 'Settings' directory of current location in Deviant Dock.
            bool portable = true;               // For creatings a setup version, just use "portable = false" or for creating portable version use "portable = true"

            if (!portable)
                path = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + @"\Deviant Dock\Settings\";
            else
                path = "Settings/";
        }
    }
}
