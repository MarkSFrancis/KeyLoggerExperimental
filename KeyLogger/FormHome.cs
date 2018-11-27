using System;
using System.Windows.Forms;

namespace KeyLogger
{
    public partial class FormHome : Form
    {
        public FormHome(KeyListener listener)
        {
            InitializeComponent();

            Listener = listener;
            Listener.KeyPressedEvent += Listener_KeyPressedEvent;
            Listener.IsListening = true;

            KeyFormatter = new KeyFormatter();
        }

        private void Listener_KeyPressedEvent(int keyCode, bool keyDown)
        {
            var formatted = KeyFormatter.FormatKeyPress(keyCode, keyDown);

            TxtLog.AppendText(formatted);
        }

        public KeyListener Listener { get; }
        public KeyFormatter KeyFormatter { get; }

        private void BtnClear_Click(object sender, EventArgs e)
        {
            TxtLog.Clear();
        }
    }
}
