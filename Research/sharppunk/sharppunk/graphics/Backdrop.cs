using System.Drawing;

namespace sharppunk.graphics
{
    public class Backdrop : Graphic
    {
        public Backdrop(Bitmap texture, bool repeatX = true, bool repeatY = true)
        {
            this.repeatX = repeatX;
            this.repeatY = repeatY;
            this.textWidth = (uint)texture.Width;
            this.textHeight = (uint)texture.Height;
            this.texture = texture;
        }

        /** @private Renders the Backdrop. */

        public override void Render(Bitmap target, Vector2 point, Vector2 camera)
        {
            if (texture == null) return;

            Vector2 origin = new Vector2();

            point += origin;

            //target.Draw( texture, point, new Rectangle(0,0,(int)textWidth,(int)textHeight), Color.White, 0,
            //			origin, 1.0f, SpriteEffects.None, 0f );
        }

        // Backdrop information.
        /** @private */
        private Bitmap texture;
        /** @private */
        private uint textWidth;
        /** @private */
        private uint textHeight;
        /** @private */
        private bool repeatX;
        /** @private */
        private bool repeatY;
        /** @private */
        private float x;
        /** @private */
        private float y;
    }
}