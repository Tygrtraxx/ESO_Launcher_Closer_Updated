using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using static System.Net.Mime.MediaTypeNames;
using Microsoft.Win32;


namespace _VS__CSharp__ESO_Launcher_Closer
{
    class Program
    {
        private static void SetStartup()
        {
            RegistryKey rk = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run", true);
            string path;
            path = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().GetName().CodeBase);
            if (rk.GetValue("ESO Launcher Closer") == null) rk.SetValue("ESO Launcher Closer", path);
        }

        static void Main(string[] args)
        {
            SetStartup();
            bool shouldClose = false;
            Process activeGame = null;

            while (true)
            {
                Process[] processes = Process.GetProcesses();
                foreach (Process p in processes)
                {
                    if (!string.IsNullOrEmpty(p.MainWindowTitle))
                    {
                        if(!shouldClose && p.MainWindowTitle == "Elder Scrolls Online")
                        {
                            shouldClose = true;
                            activeGame = p;
                        }
                        else if(shouldClose && p.MainWindowTitle == "Launcher")
                        {
                            shouldClose = false;
                            p.Kill();
                            p.Dispose();
                            SystrayRefresher.RefreshTrayArea();
                            if (activeGame != null) activeGame.WaitForExit();
                        }
                    }
                }
                System.Threading.Thread.Sleep(2000);
            }
        }
    }
}
