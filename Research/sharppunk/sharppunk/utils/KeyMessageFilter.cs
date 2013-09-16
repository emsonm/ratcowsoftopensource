using System.Collections.Generic;

namespace sharppunk.Utils
{
    //    private KeyMessageFilter m_filter = new KeyMessageFilter();;

    //private void Form1_Load(object sender, EventArgs e)
    //{
    //    Application.AddMessageFilter(m_filter);

    //}

    public class KeyMessageFilter : System.Windows.Forms.IMessageFilter
    {
        private Dictionary<System.Windows.Forms.Keys, bool> m_keyTable = new Dictionary<System.Windows.Forms.Keys, bool>();

        public Dictionary<System.Windows.Forms.Keys, bool> KeyTable
        {
            get { return m_keyTable; }
            private set { m_keyTable = value; }
        }

        public bool IsKeyPressed()
        {
            return m_keyPressed;
        }

        public bool IsKeyPressed(System.Windows.Forms.Keys k)
        {
            bool pressed = false;

            if (KeyTable.TryGetValue(k, out pressed))
            {
                return pressed;
            }

            return false;
        }

        private const int WM_KEYDOWN = 0x0100;

        private const int WM_KEYUP = 0x0101;

        private bool m_keyPressed = false;

        public bool PreFilterMessage(ref System.Windows.Forms.Message m)
        {
            if (m.Msg == WM_KEYDOWN)
            {
                KeyTable[(System.Windows.Forms.Keys)m.WParam] = true;

                m_keyPressed = true;
            }

            if (m.Msg == WM_KEYUP)
            {
                KeyTable[(System.Windows.Forms.Keys)m.WParam] = false;

                m_keyPressed = false;
            }

            return false;
        }
    }
}