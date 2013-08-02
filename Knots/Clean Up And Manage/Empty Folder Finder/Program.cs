﻿using System;
using System.Diagnostics;
using System.Threading;
using System.Windows.Forms;
using FreemiumUtil;
using System.IO;
using System.Reflection;

/// <summary>
/// The <see cref="EmptyFolderFinder"/> namespace defines an Empty folder finder knot
/// </summary>
namespace EmptyFolderFinder
{
    internal static class Program
    {
        static Mutex mutex;
        static bool created;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            mutex = new Mutex(true, Process.GetCurrentProcess().ProcessName, out created);
            if (created)
            {
                
                // As all first run initialization is done in the main project,
                // we need to make sure the user does not start a different knot first.
                if (CfgFile.Get("FirstRun") != "0")
                {
                    try
                    {
#if PCCleaner
                        ProcessStartInfo process = new ProcessStartInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\PCCleaner.exe");
#else
                        ProcessStartInfo process = new ProcessStartInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase) + "\\FreemiumUtilities.exe");
#endif
                        Process.Start(process);
                    }
                    catch (Exception)
                    {
                    }

                    Application.Exit();
                    return;
                }

                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new FormEmptyFolderFinder());
            }
        }

    
    }
}