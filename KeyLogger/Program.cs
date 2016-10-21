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
            try
            {
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new frmHome());
            }
            catch
            {
                Thread.Sleep(500);
                StartProgram();
            }
        }
    }
}
