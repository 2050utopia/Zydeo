﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Threading;
using System.Reflection;
using System.ServiceProcess;
using System.Diagnostics;
using System.IO;

namespace ZD.AU
{
    internal static class Program
    {
        [DllImport("user32.dll")]
        private static extern bool SetProcessDPIAware();

        /// <summary>
        /// The running "official" service, which will stop itself after re-launching from TEMP folder
        /// </summary>
        public static ServiceBase ServiceToRun = null;
        
        /// <summary>
        /// True if running in debugger - used to test UI directly.
        /// </summary>
        private static bool inDebugger;

        /// <summary>
        /// The form shown for downloading/installing and update.
        /// </summary>
        private static ZydeoUpdateForm uf = null;

        /// <summary>
        /// File to delete when update UI closes or crashes.
        /// </summary>
        private static string fileToDelete = null;

        /// <summary>
        /// Installs the AU helper service on this system.
        /// </summary>
        private static void doInstallService()
        {
            try
            {
                // Make sure registry is salted.
                Salt.EnsureSalt();

                // Simply exit if service has already been installed
                if (Helper.IsServiceRegistered()) return;

                // Install service; set security permissions and startup mode
                string assLoc = Assembly.GetExecutingAssembly().Location;
                ServiceInstaller.InstallService(assLoc, Magic.ServiceShortName, Magic.ServiceDisplayName);
                ServiceInstaller.ChangeStartMode(Magic.ServiceShortName, ServiceStartMode.Manual);
                ServiceInstaller.SetServiceSDDL(Magic.ServiceShortName, System.Security.AccessControl.SecurityInfos.DiscretionaryAcl, ServiceInstaller.AuSvcDaclSDDL);
                Environment.ExitCode = 0;
            }
            catch (Exception ex)
            {
                FileLogger.Instance.LogError(ex, "Failed to install service");
                Environment.ExitCode = -1;
            }
        }

        /// <summary>
        /// Uninstalls the AU helper service on this system.
        /// </summary>
        private static void doUninstallService()
        {
            try
            {
                if (!ServiceInstaller.UnInstallService(Magic.ServiceShortName))
                    throw new Exception("UninstallService call failed.");
                Environment.ExitCode = 0;
            }
            catch (Exception ex)
            {
                FileLogger.Instance.LogError(ex, "Failed to uninstall service");
                Environment.ExitCode = -1;
            }
        }

        /// <summary>
        /// Re-launches the update UI from a TEMP folder.
        /// </summary>
        private static void doLaunchUpdateFromTemp()
        {
            // Running from original location, launch ourselves from temp
            try
            {
                Helper.StartFromTemp();
            }
            catch (Exception ex)
            {
                FileLogger.Instance.LogError(ex, "Failed to launch update from TEMP folder.");
                Environment.ExitCode = -1;
            }
        }

        /// <summary>
        /// Starts the installed AU helper service. It will relaunch from TEMP folder and stop right away.
        /// Relaunched EXE from TEMP folder listens for our requests through named pipe.
        /// Still running as LOCAL SYSTEM, it has privileges to run our installer after verifying it.
        /// </summary>
        /// <returns>True if service started; false if couldn't start service.</returns>
        private static bool doStartService()
        {
            FileLogger.Instance.LogInfo("Starting service.");
            try
            {
                using (ServiceController sc = new ServiceController(Magic.ServiceShortName))
                {
                    sc.Start();
                    return true;
                }
            }
            catch (Exception ex)
            {
                FileLogger.Instance.LogError(ex, "Failed to start service.");
                return false;
            }
        }

        /// <summary>
        /// Listens to update UI's wishes and exectures installer if everything's OK.
        /// </summary>
        private static void doServiceWork()
        {
            string pname = Process.GetCurrentProcess().ProcessName;
            FileLogger.Instance.LogInfo("Service starting from temp location [" + pname + "]");
            try
            {
                Service.Work();
            }
            catch (Exception ex)
            {
                FileLogger.Instance.LogError(ex, "Service.Work() terminated with an error.");
            }
        }

        /// <summary>
        /// Schedules a file for deletion before we quit or crash. This is how we clean up installer downloded by UI.
        /// </summary>
        private static void doScheduleFileToDelete(string fname)
        {
            fileToDelete = fname;
        }

        /// <summary>
        /// Deletes scheduled file; never throws.
        /// </summary>
        private static void doSafeDeleteScheduledFile()
        {
            try
            {
                if (fileToDelete != null)
                {
                    Helper.SafeDeleteFile(fileToDelete);
                    fileToDelete = null;
                }
            }
            catch { }
        }

        /// <summary>
        /// Shows the update UI.
        /// </summary>
        private static void doUpdateForm(bool serviceStartedForUI)
        {
            FileLogger.Instance.LogInfo("Showing form.");
            if (Environment.OSVersion.Version.Major >= 6) SetProcessDPIAware();
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            uf = new ZydeoUpdateForm(doScheduleFileToDelete, serviceStartedForUI);
            Application.Run(uf);
            // When quitting gracefully, delete file
            // But never throw on this attempt
            doSafeDeleteScheduledFile();
        }

        /// <summary>
        /// <para>Signs an update URL and and update package with the provided private key.</para>
        /// <para>The private key belongs to the embedded public key that we use for verification, but it's NOT included in the code base.</para>
        /// </summary>
        private static void doSignUpdate(string[] args)
        {
            if (args.Length != 5)
                throw new Exception("Expected usage: /sign <url> <installer-file> <private-key-file> <output-file>");

            using (StreamReader srKey = new StreamReader(args[3]))
            using (StreamWriter srOut = new StreamWriter(args[4]))
            {
                string keyXml = srKey.ReadToEnd();
                string sigUrl = SignatureCheck.Sign(args[1], keyXml);
                string sigFile = SignatureCheck.Sign(new FileInfo(args[2]), keyXml);
                srOut.WriteLine("URL signature:");
                srOut.WriteLine(sigUrl);
                srOut.WriteLine();
                srOut.WriteLine("File signature:");
                srOut.WriteLine(sigFile);
                srOut.WriteLine();
            }
        }

        /// <summary>
        /// Main entry point, within the exception cushion.
        /// </summary>
        /// <param name="args"></param>
        private static void mainCore(string[] args)
        {
            // No self-copying when runnung a debug build, in the debugger
            inDebugger = Debugger.IsAttached;
#if !DEBUG
            inDebugger = false;
#endif

            if (args.Length > 0)
            {
                switch (args[0])
                {
                    case "/install":
                        doInstallService();
                        return;
                    case "/uninstall":
                        doUninstallService();
                        return;
                    case "/update":
                        // Only re-launching from temp if we're not debugging. Otherwise, run straight.
                        if (!inDebugger)
                        {
                            doLaunchUpdateFromTemp();
                            return;
                        }
                        break;
                    case "/sign":
                        doSignUpdate(args);
                        return;
               }
            }

            // No arguments, running in debugger: just do service work
            if (inDebugger && args.Length == 0)
            {
                doServiceWork();
                return;
            }

            // No arguments, I'm a service, but not running from temp >> relaunch from temp
            if (Helper.IsService() && !Helper.IsRunningFromTemp())
            {
                // Running from original location
                // Start the service, which will re-launch itself from temp location
                ServiceToRun = new Service();
                ServiceBase.Run(ServiceToRun);
                return;
            }

            // At this point we must be the user process
            // This should never happen.
            if (!inDebugger && !Helper.IsRunningFromTemp())
            {
                Environment.ExitCode = -1;
                return;
            }

            // If we're running form temporary folder, schedule ourselves for demolition, sorry, deletion
            Helper.DeleteAfterRebootIfTemp();

            // OK, we're definitely running from temp folder now. (Or debugging.)
            // If we're the UI client, fire up service
            bool serviceStartedForUI = false;
            if (!Helper.IsService()) serviceStartedForUI = doStartService();

            // Running from temp as either SYSTEM or user
            // Wait until parent process exists. Parent's process ID is passed onto us as the first cmdline argument
            // BUT: don't do this in debugger; there we're running "as is", no PID argument passed.
            if (args.Length == 0) return;
            if (!inDebugger)
            {
                int parentProcessId;
                if (!int.TryParse(args[0], out parentProcessId))
                {
                    Environment.ExitCode = -1;
                    return;
                }
                Helper.WaitForProcessExit(parentProcessId);
            }

            if (Helper.IsService()) doServiceWork();
            else doUpdateForm(serviceStartedForUI);
        }

        /// <summary>
        /// Main entry point. Cushions actual main function with exception handling.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            // Touch error logger just to initialize it
            var x = FileLogger.Instance;
            AppDomain.CurrentDomain.UnhandledException += onUnhandledException;
            mainCore(args);
        }

        /// <summary>
        /// Handles unhandled app domain exceptions.
        /// </summary>
        private static void onUnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            // We must do the processing in a different thread that is STA
            // Source of exception may be worker thread, which is MTA
            // MTA cannot show UI, and that's precisely what we want to do.
            Thread thread = new Thread(doHandleExceptionLastResort);
            thread.SetApartmentState(ApartmentState.STA);
            thread.Start(e.ExceptionObject);
            // Must wait for thread to complete.
            // Otherwise, running timers and other thread may throw again
            // And then we go straight against the wall with a Windows error report.
            thread.Join();
            Environment.Exit(-1);
        }

        /// <summary>
        /// Last-resourt exception handler. Executed on separate thread.
        /// </summary>
        /// <param name="o"></param>
        private static void doHandleExceptionLastResort(object o)
        {
            // Before dealing with exception: delete temp file from UI.
            doSafeDeleteScheduledFile();

            // Deal with exception.
            Exception ex = null;
            // About all the swallowed exceptions.
            // We have already thrown. If the handler throws too, all bets are really of - nothing left to do here.
            try
            {
                // Get out exception object. I don't think this can fail, but still safer inside try block.
                ex = o as Exception;
                // Close main window
                if (uf != null) uf.ForceClose();
            }
            catch { }
            // Log error to file. This we know is thread-safe.
            FileLogger.Instance.LogException(ex);
        }
    }
}
