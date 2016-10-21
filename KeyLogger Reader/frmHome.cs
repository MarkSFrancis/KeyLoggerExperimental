using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;

namespace KeyLogger_Reader
{
    public partial class frmHome : Form
    {
        public frmHome()
        {
            InitializeComponent();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            if (ofdImport.ShowDialog() == DialogResult.OK)
            {
                using (StreamReader reader = new StreamReader(ofdImport.FileName))
                {
                    txtMain.Text = TranslateData(reader.ReadToEnd());
                }
            }
        }

        string TranslateData(string Data)
        {
            string ReturnValue = "";

            foreach (string s in Data.Split(','))
            {
                string b = s.Substring(2);
                if (b.Contains("|"))
                {
                    string Pressed;

                    if (b.Substring(b.IndexOf('|') + 1) == "true")
                        Pressed = "{&Pressed Down}";
                    else
                        Pressed = "{&Released}";

                    ReturnValue += string.Format("{0}{1}", DataAnalysis.KeyToString(StringToInt(
                        b.Substring(0, s.IndexOf('|')))), Pressed);
                }
                else
                    ReturnValue += DataAnalysis.KeyToString(StringToInt(b));
            }

            return ReturnValue;
        }

        readonly char[] SomeNumbers = { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9' }; //Used to validate string as a number
        int StringToInt(string Text)
        {
            if (Text == null || Text == "")
            {
                return 0;
            }

            int ReturnValue = 0;
            int i = 0; //Used to ignore "-" character in for loop
            bool negative;

            if (Text[0] == '-')
            {
                negative = true;
                i = 1;
            }
            else
            {
                negative = false;
            }

            for (; i < Text.Length; i++)
            {
                foreach (char c2 in SomeNumbers)
                {
                    if (Text[i] == c2)
                    {
                        ReturnValue = (ReturnValue * 10) + (Text[i] - '0');
                        break;
                    }
                }
            }

            if (negative)
            {
                ReturnValue = 0 - ReturnValue;
            }

            return ReturnValue;
        }

        private void btnDetectPasswords_Click(object sender, EventArgs e)
        {

        }
    }

    class DataAnalysis
    {
        public enum VirtualKeyCodes : int
        {
            None = 0,
            LeftMouse = 1,
            RightMouse = 2,
            Cancel = 3,
            MiddleMouse = 4,
            x1Mouse = 5,
            x2Mouse = 6,
            //Some undefined keys here
            Backspace = 8,
            Tab = 9,
            //Some undefined keys here
            Clear = 12,
            ReturnEnter = 13,
            //Some undefined keys here
            Shift = 16,
            Control = 17,
            Alt = 18,
            Pause = 19,
            CapsLock = 20,
            IMEHangul = 21,
            //Some undefined keys here
            IMEJunga = 23,
            IMEFinal = 24,
            IMEHanja = 25,
            //Some undefined keys here
            Escape = 27,
            IMEConvert = 28,
            IMENonConvert = 29,
            IMEAccept = 30,
            IMEModeChange = 31,
            SpaceBar = 32,
            PageUp = 33,
            PageDown = 34,
            End = 35,
            Home = 36,
            LeftArrow = 37,
            UpArrow = 38,
            RightArrow = 39,
            DownArrow = 40,
            Select = 41,
            Print = 42,
            Execute = 43,
            PrintScreen = 44,
            Insert = 45,
            Deleted = 46,
            Help = 47,
            Zero = 48,
            One = 49,
            Two = 50,
            Three = 51,
            Four = 52,
            Five = 53,
            Six = 54,
            Seven = 55,
            Eight = 56,
            Nine = 57,
            //Some undefined keys here
            A = 65,
            B = 66,
            C = 67,
            D = 68,
            E = 69,
            F = 70,
            G = 71,
            H = 72,
            I = 73,
            J = 74,
            K = 75,
            L = 76,
            M = 77,
            N = 78,
            O = 79,
            P = 80,
            Q = 81,
            R = 82,
            S = 83,
            T = 84,
            U = 85,
            V = 86,
            W = 87,
            X = 88,
            Y = 89,
            Z = 90,
            LeftWin = 91,
            RightWin = 92,
            Applications = 93,
            //Some undefined keys here
            Sleep = 95,
            NumZero = 96,
            NumOne = 97,
            NumTwo = 98,
            NumThree = 99,
            NumFour = 100,
            NumFive = 101,
            NumSix = 102,
            NumSeven = 103,
            NumEight = 104,
            NumNine = 105,
            Multiply = 106,
            Add = 107,
            Seperator = 108,
            Subtract = 109,
            Decimal = 110,
            Divide = 111,
            F1 = 112,
            F2 = 113,
            F3 = 114,
            F4 = 115,
            F5 = 116,
            F6 = 117,
            F7 = 118,
            F8 = 119,
            F9 = 120,
            F10 = 121,
            F11 = 122,
            F12 = 123,
            F13 = 124,
            F14 = 125,
            F15 = 126,
            F16 = 127,
            F17 = 128,
            F18 = 129,
            F19 = 130,
            F20 = 131,
            F21 = 132,
            F22 = 133,
            F23 = 134,
            F24 = 135,
            //Some undefined keys here
            NumLock = 144,
            ScrollLock = 145,
            //Some undefined keys here
            LShift = 160,
            RShift = 161,
            LControl = 162,
            RControl = 163,
            LAlt = 164,
            RAlt = 165,
            BrowserBack = 166,
            BrowserForward = 167,
            BrowserRefresh = 168,
            BrowserStop = 169,
            BrowserSearch = 170,
            BrowserFavourites = 171,
            BrowserHome = 172,
            VolumeMute = 173,
            VolumeDown = 174,
            VolumeUp = 175,
            MediaNextTrack = 176,
            MediaPreviousTrack = 177,
            MediaStop = 178,
            MediaPlayPause = 179,
            LaunchMail = 180,
            LauchMediaSelect = 181,
            LaunchApp1 = 182,
            LaunchApp2 = 183,
            SemiColon = 186,
            Plus = 187,
            Comma = 188,
            Minus = 189,
            Period = 190,
            ForwardSlash = 191,
            SingleQuote = 192,
            SquareBracketLeft = 219,
            BackSlash = 220,
            SquareBracketRight = 221,
            Hastag = 222,
            AngledQuote = 223,
            //Some undefined keys here
            VKPlay = 250,
            VKZoom = 251
            //Some undefined keys here
        }

        static Dictionary<VirtualKeyCodes, string> KeyNameDictionary = new Dictionary<VirtualKeyCodes, string>()
        {
            { VirtualKeyCodes.LeftMouse, "{Left Mouse}" },
            { VirtualKeyCodes.RightMouse, "{Right Mouse}" },
            { VirtualKeyCodes.Cancel, "{Cancel}" },
            { VirtualKeyCodes.MiddleMouse, "{Middle Mouse}" },
            { VirtualKeyCodes.x1Mouse, "{X1 Mouse}" },
            { VirtualKeyCodes.x2Mouse, "{X2 Mouse}" },
            { VirtualKeyCodes.Backspace, "{Backspace}" },
            { VirtualKeyCodes.Tab, "{Tab}" },
            { VirtualKeyCodes.Clear, "{Clear}" },
            { VirtualKeyCodes.ReturnEnter, "\r\n" },
            { VirtualKeyCodes.Shift, "{Shift}" },
            { VirtualKeyCodes.Control, "{Control}" },
            { VirtualKeyCodes.Alt, "{Alt}" },
            { VirtualKeyCodes.Pause, "{Pause}" },
            { VirtualKeyCodes.CapsLock, "{CapsLock}" },
            { VirtualKeyCodes.IMEHangul, "{IMEHangul}" },
            { VirtualKeyCodes.IMEJunga, "{IMEJunga}" },
            { VirtualKeyCodes.IMEFinal, "{IMEFinal}" },
            { VirtualKeyCodes.IMEHanja, "{IMEHanja}" },
            { VirtualKeyCodes.Escape, "{Escape}" },
            { VirtualKeyCodes.IMEConvert, "{IMEConvert}" },
            { VirtualKeyCodes.IMENonConvert, "{IMENonConvert}" },
            { VirtualKeyCodes.IMEAccept, "{IMEAccept}" },
            { VirtualKeyCodes.IMEModeChange, "{IMEModeChange}" },
            { VirtualKeyCodes.SpaceBar, " " },
            { VirtualKeyCodes.PageUp, "{Page Up}" },
            { VirtualKeyCodes.PageDown, "{Page Down}" },
            { VirtualKeyCodes.End, "{End}" },
            { VirtualKeyCodes.Home, "{Home}" },
            { VirtualKeyCodes.LeftArrow, "{Left Arrow}" },
            { VirtualKeyCodes.UpArrow, "{Up Arrow}" },
            { VirtualKeyCodes.RightArrow, "{Right Arrow}" },
            { VirtualKeyCodes.DownArrow, "{Down Arrow}" },
            { VirtualKeyCodes.Select, "{Select}" },
            { VirtualKeyCodes.Print, "{Print}" },
            { VirtualKeyCodes.Execute, "{Execute}" },
            { VirtualKeyCodes.PrintScreen, "{Print Screen}" },
            { VirtualKeyCodes.Insert, "{Insert}" },
            { VirtualKeyCodes.Deleted, "{Deleted}" },
            { VirtualKeyCodes.Help, "{Help}" },
            { VirtualKeyCodes.Zero, "0" },
            { VirtualKeyCodes.One, "1" },
            { VirtualKeyCodes.Two, "2" },
            { VirtualKeyCodes.Three, "3" },
            { VirtualKeyCodes.Four, "4" },
            { VirtualKeyCodes.Five, "5" },
            { VirtualKeyCodes.Six, "6" },
            { VirtualKeyCodes.Seven, "7" },
            { VirtualKeyCodes.Eight, "8" },
            { VirtualKeyCodes.Nine, "9" },
            { VirtualKeyCodes.A, "a" },
            { VirtualKeyCodes.B, "b" },
            { VirtualKeyCodes.C, "c" },
            { VirtualKeyCodes.D, "d" },
            { VirtualKeyCodes.E, "e" },
            { VirtualKeyCodes.F, "f" },
            { VirtualKeyCodes.G, "g" },
            { VirtualKeyCodes.H, "h" },
            { VirtualKeyCodes.I, "i" },
            { VirtualKeyCodes.J, "j" },
            { VirtualKeyCodes.K, "k" },
            { VirtualKeyCodes.L, "l" },
            { VirtualKeyCodes.M, "m" },
            { VirtualKeyCodes.N, "n" },
            { VirtualKeyCodes.O, "o" },
            { VirtualKeyCodes.P, "p" },
            { VirtualKeyCodes.Q, "q" },
            { VirtualKeyCodes.R, "r" },
            { VirtualKeyCodes.S, "s" },
            { VirtualKeyCodes.T, "t" },
            { VirtualKeyCodes.U, "u" },
            { VirtualKeyCodes.V, "v" },
            { VirtualKeyCodes.W, "w" },
            { VirtualKeyCodes.X, "x" },
            { VirtualKeyCodes.Y, "y" },
            { VirtualKeyCodes.Z, "z" },
            { VirtualKeyCodes.LeftWin, "{Left Windows Key}" },
            { VirtualKeyCodes.RightWin, "{Right Windows Key}" },
            { VirtualKeyCodes.Applications, "{Applications}" },
            { VirtualKeyCodes.Sleep, "{Sleep}" },
            { VirtualKeyCodes.NumZero, "0" },
            { VirtualKeyCodes.NumOne, "1" },
            { VirtualKeyCodes.NumTwo, "2" },
            { VirtualKeyCodes.NumThree, "3" },
            { VirtualKeyCodes.NumFour, "4" },
            { VirtualKeyCodes.NumFive, "5" },
            { VirtualKeyCodes.NumSix, "6" },
            { VirtualKeyCodes.NumSeven, "7" },
            { VirtualKeyCodes.NumEight, "8" },
            { VirtualKeyCodes.NumNine, "9" },
            { VirtualKeyCodes.Multiply, "{Muliiply}" },
            { VirtualKeyCodes.Add, "{Add}" },
            { VirtualKeyCodes.Seperator, "{Seperator}" },
            { VirtualKeyCodes.Subtract, "{Subtract}" },
            { VirtualKeyCodes.Decimal, "{Decimal}" },
            { VirtualKeyCodes.Divide, "{Divide}" },
            { VirtualKeyCodes.F1, "{F1}" },
            { VirtualKeyCodes.F2, "{F2}" },
            { VirtualKeyCodes.F3, "{F3}" },
            { VirtualKeyCodes.F4, "{F4}" },
            { VirtualKeyCodes.F5, "{F5}" },
            { VirtualKeyCodes.F6, "{F6}" },
            { VirtualKeyCodes.F7, "{F7}" },
            { VirtualKeyCodes.F8, "{F8}" },
            { VirtualKeyCodes.F9, "{F9}" },
            { VirtualKeyCodes.F10, "{F10}" },
            { VirtualKeyCodes.F11, "{F11}" },
            { VirtualKeyCodes.F12, "{F12}" },
            { VirtualKeyCodes.F13, "{F13}" },
            { VirtualKeyCodes.F14, "{F14}" },
            { VirtualKeyCodes.F15, "{F15}" },
            { VirtualKeyCodes.F16, "{F16}" },
            { VirtualKeyCodes.F17, "{F17}" },
            { VirtualKeyCodes.F18, "{F18}" },
            { VirtualKeyCodes.F19, "{F19}" },
            { VirtualKeyCodes.F20, "{F20}" },
            { VirtualKeyCodes.F21, "{F21}" },
            { VirtualKeyCodes.F22, "{F22}" },
            { VirtualKeyCodes.F23, "{F23}" },
            { VirtualKeyCodes.F24, "{F24}" },
            { VirtualKeyCodes.NumLock, "{Num Lock}" },
            { VirtualKeyCodes.ScrollLock, "{Scroll Lock}" },
            { VirtualKeyCodes.LShift, "{Left Shift}" },
            { VirtualKeyCodes.RShift, "{Right Shift}" },
            { VirtualKeyCodes.LControl, "{Left Control}" },
            { VirtualKeyCodes.RControl, "{Right Control}" },
            { VirtualKeyCodes.LAlt, "{Left Alt}" },
            { VirtualKeyCodes.RAlt, "{Right Alt}" },
            { VirtualKeyCodes.BrowserBack, "{Browser Back}" },
            { VirtualKeyCodes.BrowserForward, "{Browser Forward}" },
            { VirtualKeyCodes.BrowserRefresh, "{Browser Refresh}" },
            { VirtualKeyCodes.BrowserStop, "{Browser Stop}" },
            { VirtualKeyCodes.BrowserSearch, "{Browser Search}" },
            { VirtualKeyCodes.BrowserFavourites, "{Browser Favourites}" },
            { VirtualKeyCodes.BrowserHome, "{Browser Home}" },
            { VirtualKeyCodes.VolumeMute, "{Volume Mute}" },
            { VirtualKeyCodes.VolumeDown, "{Volume Down}" },
            { VirtualKeyCodes.VolumeUp, "{Volume Up}" },
            { VirtualKeyCodes.MediaNextTrack, "{Media Next Track}" },
            { VirtualKeyCodes.MediaPreviousTrack, "{Media Previous Track}" },
            { VirtualKeyCodes.MediaStop, "{Media Stop}" },
            { VirtualKeyCodes.MediaPlayPause, "{Media Play Pause}" },
            { VirtualKeyCodes.LaunchMail, "{Launch Mail}" },
            { VirtualKeyCodes.LauchMediaSelect, "{Launch Media Select}" },
            { VirtualKeyCodes.LaunchApp1, "{Launch App1}" },
            { VirtualKeyCodes.LaunchApp2, "{Launch App2}" },
            { VirtualKeyCodes.SemiColon, ";" },
            { VirtualKeyCodes.Plus, "=" },
            { VirtualKeyCodes.Comma, "," },
            { VirtualKeyCodes.Minus, "-" },
            { VirtualKeyCodes.Period, "." },
            { VirtualKeyCodes.ForwardSlash, "/" },
            { VirtualKeyCodes.SingleQuote, "'" },
            { VirtualKeyCodes.SquareBracketLeft, "[" },
            { VirtualKeyCodes.BackSlash, "\\" },
            {VirtualKeyCodes.SquareBracketRight, "]" },
            { VirtualKeyCodes.Hastag, "#" },
            { VirtualKeyCodes.AngledQuote, "`" },
            { VirtualKeyCodes.VKPlay, "{VKPlay}" },
            { VirtualKeyCodes.VKZoom, "{VKZoom}" }
        };

        public static string KeyToString(int i)
        {
            if (i == 0)
                return "";

            try
            {
                return KeyNameDictionary[(VirtualKeyCodes)i];
            }
            catch
            {
                return string.Format("{Key Not Found: {0}}", i);
            }
        }
    }
}
