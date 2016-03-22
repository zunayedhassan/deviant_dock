using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using IWshRuntimeLibrary;

namespace Deviant_Dock
{
    class LauncherInfo
    {
        public string target;

        public LauncherInfo(string shortCutFileName)
        {
            if (new FileInfo(shortCutFileName).Extension == ".lnk")
            {
                WshShell shell = new WshShell();
                WshShortcut shortcut = (WshShortcut)shell.CreateShortcut(shortCutFileName);

                try
                {
                    if ((new FileInfo(shortcut.TargetPath).Extension == ".exe"))
                        this.target = shortcut.TargetPath;
                }
                catch (Exception extensionException)
                {
                    // Do nothing
                }
            }
            else
            {
                switch (shortCutFileName)
                {
                    case @"C:\Windows\Installer\{90140000-0011-0000-0000-0000000FF1CE}\wordicon.exe":
                        this.target = @"C:\Program Files\Microsoft Office\Office14\WINWORD.EXE";
                        break;

                    case @"C:\Windows\Installer\{90140000-0011-0000-0000-0000000FF1CE}\xlicons.exe":
                        this.target = @"C:\Program Files\Microsoft Office\Office14\EXCEL.EXE";
                        break;

                    case @"C:\Windows\Installer\{90140000-0011-0000-0000-0000000FF1CE}\pptico.exe":
                        this.target = @"C:\Program Files\Microsoft Office\Office14\POWERPNT.EXE";
                        break;

                    case @"C:\Windows\Installer\{90140000-0011-0000-0000-0000000FF1CE}\accicons.exe":
                        this.target = @"C:\Program Files\Microsoft Office\Office14\MSACCESS.EXE";
                        break;

                    case @"C:\Windows\Installer\{90140000-0011-0000-0000-0000000FF1CE}\outicon.exe":
                        this.target = @"C:\Program Files\Microsoft Office\Office14\OUTLOOK.EXE";
                        break;

                    case @"C:\Windows\Installer\{90140000-0011-0000-0000-0000000FF1CE}\joticon.exe":
                        this.target = @"C:\Program Files\Microsoft Office\Office14\ONENOTE.EXE";
                        break;

                    case @"C:\Windows\Installer\{90140000-0011-0000-0000-0000000FF1CE}\pubs.exe":
                        this.target = @"C:\Program Files\Microsoft Office\Office14\MSPUB.EXE";
                        break;

                    case @"C:\Windows\Installer\{90140000-0057-0000-0000-0000000FF1CE}\visicon.exe":
                        this.target = @"C:\Program Files\Microsoft Office\Office14\VISIO.EXE";
                        break;

                    case @"C:\Windows\Installer\{90140000-003B-0000-0000-0000000FF1CE}\pj11icon.exe":
                        this.target = @"C:\Program Files\Microsoft Office\Office14\WINPROJ.EXE";
                        break;

                    default:
                        this.target = shortCutFileName;
                        break;
                }
            }
        }
    }
}
