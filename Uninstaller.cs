using System;
using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace ESO_Launcher_Closer_Uninstaller
{
    class Program
    {
        static void Main(string[] args)
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            string path = Path.ChangeExtension(Application.ExecutablePath, ".exe");
            if (rk.GetValue("ESO Launcher Closer") != null) rk.DeleteValue("ESO Launcher Closer");

            var proc = Process.GetProcessesByName("ESO Launcher Closer");
            foreach(var p in proc)
            {
                p.Kill();
                p.Dispose();
            }
        }
    }
}
