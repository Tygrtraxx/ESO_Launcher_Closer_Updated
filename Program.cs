using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;

namespace _VS__CSharp__ESO_Launcher_Closer
{
    class Program
    {

        static void Main(string[] args)
        {
            bool shouldClose = true;
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
