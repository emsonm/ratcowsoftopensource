using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RatCow.Controls
{
    public class SelectionControl : FocusControl
    {
        public SelectionControl()
            : base()
        {
            SelectedColor = System.Drawing.Color.ForestGreen;
            _cachedEnabledColor = EnabledColor; //this won't work if the enabled color changes
        }

        public bool Selected { get; set; }
        public System.Drawing.Color SelectedColor { get; set; }
        private System.Drawing.Color _cachedEnabledColor;

        public void Select(bool selected = true)
        {
            Selected = selected;

            if (Selected)
            {
                _cachedEnabledColor = EnabledColor;
                EnabledColor = SelectedColor;
            }
            else
            {
                EnabledColor = _cachedEnabledColor;
            }
        }

        public override bool HitTest(int x, int y, bool? mouseIsDown)
        {
            var result = base.HitTest(x, y, mouseIsDown);

            if (result)
            {
                Selected = true;
            }

            return result;
        }
    }
}
