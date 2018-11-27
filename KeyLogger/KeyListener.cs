using System;
using System.Runtime.InteropServices;

namespace KeyLogger
{
    public class KeyListener : IDisposable
    {
        public KeyListener()
        {
            _hInstance = LoadLibrary("User32");
        }

        private readonly IntPtr _hInstance;
        private IntPtr? _currentHookId;

        private delegate IntPtr LowLevelKeyboardProc(int nCode, IntPtr wParam, IntPtr lParam);
        public delegate void KeyPressedEventHandler(int keyCode, bool keyDown);

        public event KeyPressedEventHandler KeyPressedEvent;

        public bool IsListening
        {
            get => _currentHookId.HasValue;
            set
            {
                if (value)
                {
                    if (_currentHookId.HasValue) return;

                    _currentHookId = SetWindowsHookEx((int)WindowsSignals.KeyPressEventId, KeyPressHandler, _hInstance, 0);
                }
                else
                {
                    if (!_currentHookId.HasValue) return;

                    UnhookWindowsHookEx(_currentHookId.Value);
                    _currentHookId = null;
                }
            }
        }

        private IntPtr KeyPressHandler(int code, IntPtr wParam, IntPtr lParam)
        {
            if (code >= 0)
            {
                if (wParam == (IntPtr)WindowsSignals.KeyDownEvent)
                {
                    int keyCode = Marshal.ReadInt32(lParam);

                    KeyPressedEvent?.Invoke(keyCode, true);
                }
                else if (wParam == (IntPtr)WindowsSignals.KeyUpEvent)
                {
                    int keyCode = Marshal.ReadInt32(lParam);

                    KeyPressedEvent?.Invoke(keyCode, false);
                }

                return (IntPtr)0;
            }
            else
            {
                return CallNextHookEx(_currentHookId.Value, code, (int)wParam, lParam);
            }
        }

        #region DllImports

        [DllImport("user32.dll")]
        private static extern bool UnhookWindowsHookEx(IntPtr hInstance);

        [DllImport("kernel32.dll")]
        static extern IntPtr LoadLibrary(string lpFileName);

        [DllImport("user32.dll")]
        static extern IntPtr CallNextHookEx(IntPtr idHook, int nCode, int wParam, IntPtr lParam);

        [DllImport("user32.dll")]
        static extern IntPtr SetWindowsHookEx(int idHook, LowLevelKeyboardProc callback, IntPtr hInstance, uint threadId);

        public void Dispose()
        {
            IsListening = false;
        }

        #endregion

        private enum WindowsSignals
        {
            KeyPressEventId = 13,
            KeyDownEvent = 0x100,
            KeyUpEvent = 0x101
        }
    }
}
