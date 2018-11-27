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
            StartProgram();
        }

        static void StartProgram()
        {
            bool errorOccurred;
            do
            {
                errorOccurred = false;

                using (var listener = new KeyListener())
                {
                    try
                    {
                        Application.EnableVisualStyles();
                        Application.SetCompatibleTextRenderingDefault(false);
                        Application.Run(new FormHome(listener));
                    }
                    catch
                    {
                        Thread.Sleep(500);
                        errorOccurred = true;
                    }
                }
            } while (errorOccurred);
        }
    }
}
