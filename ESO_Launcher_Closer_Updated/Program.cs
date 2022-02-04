using Microsoft.Win32;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace ESO_Launcher_Closer
{
    class Program
    {
        private static void SetStartup()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            string path = Path.ChangeExtension(Application.ExecutablePath, ".exe");
            if (rk.GetValue("ESO Launcher Closer") == null) rk.SetValue("ESO Launcher Closer", path);
        }

        static void Main(string[] args)
        {
            SetStartup();
            while (true)
            {
                Process[] games = Process.GetProcessesByName("eso64");
                if (games.Length > 0)
                {
                    Process[] launchers = Process.GetProcessesByName("Bethesda.net_Launcher");
                    foreach (var launcher in launchers)
                    {
                        launcher.Kill();
                        launcher.Dispose();
                        SystrayRefresher.RefreshTrayArea();
                    }
                    games[0].WaitForExit();
                }
                System.Threading.Thread.Sleep(2000);
            }
        }
    }
}
