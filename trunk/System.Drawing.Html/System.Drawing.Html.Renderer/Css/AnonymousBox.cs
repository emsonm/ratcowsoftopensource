using System;
using System.Collections.Generic;
using System.Text;

namespace System.Drawing.Html.Renderer.Css
{
    /// <summary>
    /// Represents an anonymous inline box
    /// </summary>
    /// <remarks>
    /// To learn more about anonymous inline boxes visit:
    /// http://www.w3.org/TR/CSS21/visuren.html#anonymous
    /// </remarks>
    public class AnonymousBox
        : Box
    {
        #region Ctor

        public AnonymousBox(Box parentBox)
            : base(parentBox)
        {

        }

        #endregion
    }

    /// <summary>
    /// Represents an anonymous inline box which contains nothing but blank spaces
    /// </summary>
    public class CssAnonymousSpaceBox
        : AnonymousBox
    {
        public CssAnonymousSpaceBox(Box parentBox)
            : base(parentBox)
        {

        }
    }

}
