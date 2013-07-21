using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RatCow.Controls
{
    public class TextControl : SelectionControl
    {

        public TextControl()
            : base()
        {
        }

        public virtual void TextInput(char key)
        {
            //Visible & Enabled & Selected &
            bool handled;
            DoKeyAction(key, out handled);
            if (!handled)
            {
                if ((int)key >= 32)
                    //need to take in to consideration the caretpositon
                    Text += key;
            }
        }

        public virtual void KeyInput(Key key, bool shift, bool control, bool altDown)
        {
            switch (key)
            {
                case Key.BackSpace:
                    var i = Text.Length - 1;
                    if (i > -1)
                        Text = Text.Remove(i);
                    break;
            }
        }

        public int CaretPosition { get; internal set; }

        public event TextInputAction KeyAction;
        private void DoKeyAction(char key, out bool handled)
        {
            if (KeyAction != null)
                KeyAction(this, key, out handled);
            else
                handled = false;
        }
    }

    public delegate void TextInputAction(object sender, char key, out bool handled);
}
