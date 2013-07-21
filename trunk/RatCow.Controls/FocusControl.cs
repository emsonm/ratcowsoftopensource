using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RatCow.Controls
{
    public class FocusControl : Control
    {
        public FocusControl()
            : base()
        {
            FocusedColor = System.Drawing.Color.Red;
            UnfocusedColor = System.Drawing.Color.CornflowerBlue;
        }
        public bool Focused { get; set; }

        public System.Drawing.Color FocusedColor { get; set; }
        public System.Drawing.Color UnfocusedColor { get; set; }

        public virtual bool HitTest(int x, int y, bool? mouseIsDown)
        {
            Focused = ((x >= Left & x <= Left + Width) & (y >= Top & y <= Top + Height));
            if (Focused & (mouseIsDown.HasValue && mouseIsDown.Value))
            {
                DoAction();
            }

            return Focused;
        }

        protected virtual void DoAction()
        {

        }
    }
}
