using System.Drawing;

namespace sharppunk.graphics
{
    public class Image : Graphic
    {
        /**
 * Rotation of the image, in degrees.
 */
        public float Angle = 0;

        /**
         * Scale of the image, affects both x and y scale.
         */
        public float Scale = 1;

        /**
         * X scale of the image.
         */
        public float ScaleX = 1;

        /**
         * Y scale of the image.
         */
        public float ScaleY = 1;

        /**
         * X origin of the image, determines transformation point.
         */
        public float OriginX = 0;

        /**
         * Y origin of the image, determines transformation point.
         */
        public float OriginY = 0;

        public bool Flipped = false;

        private Bitmap texture;

        public Image(Bitmap source)
        {
            clipRect = new Rectangle(0, 0, source.Width, source.Height);
            texture = source;
        }

        public Image(Bitmap source, Rectangle clipRect)
        {
            this.clipRect = clipRect;
            texture = source;
        }

        public override void Render(System.Drawing.Graphics target, Vector2 point, Vector2 camera)
        {
            if (texture == null) return;

            Vector2 origin = new Vector2(OriginX, OriginY);

            point += origin;

            //target.Draw ( texture, point, clipRect, Color.White, MP.Degs2Rad(Angle),
            //			origin, 1.0f, Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f );
        }

        protected Rectangle clipRect;
    }
}