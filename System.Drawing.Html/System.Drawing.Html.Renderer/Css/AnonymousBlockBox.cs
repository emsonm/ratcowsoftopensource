using System;
using System.Collections.Generic;
using System.Text;

namespace System.Drawing.Html.Renderer.Css
{
    /// <summary>
    /// Represents an anonymous block box
    /// </summary>
    /// <remarks>
    /// To learn more about anonymous block boxes visit CSS spec:
    /// http://www.w3.org/TR/CSS21/visuren.html#anonymous-block-level
    /// </remarks>
    public class AnonymousBlockBox
        : Box
    {
        public AnonymousBlockBox(Box parent)
            : base(parent)
        {
            Display = Constants.Block;
        }

        public AnonymousBlockBox(Box parent, Box insertBefore)
            : this(parent)
        {
            int index = parent.Boxes.IndexOf(insertBefore);

            if (index < 0)
            {
                throw new Exception("insertBefore box doesn't exist on parent");
            }
            parent.Boxes.Remove(this);
            parent.Boxes.Insert(index, this);
        }
    }

    /// <summary>
    /// Represents an AnonymousBlockBox which contains only blank spaces
    /// </summary>
    public class CssAnonymousSpaceBlockBox
        : AnonymousBlockBox
    {
        public CssAnonymousSpaceBlockBox(Box parent)
            : base(parent)
        { Display = Constants.None; }

        public CssAnonymousSpaceBlockBox(Box parent, Box insertBefore)
            : base(parent, insertBefore)
        { Display = Constants.None; }
    }
}
