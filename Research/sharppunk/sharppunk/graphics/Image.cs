using System.Drawing;
using System.Drawing.Drawing2D;

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

        public override void Render(Graphics target, Vector2 point, Vector2 camera)
        {
            if (texture == null) return;

            Vector2 origin = new Vector2(OriginX, OriginY);

            point += origin;

            //using (
            var g = target; //System.Drawing.Graphics.FromImage(target))
            {
                //target.Draw ( texture, point, clipRect, Color.White, MP.Degs2Rad(Angle),
                //			origin, 1.0f, Flipped ? SpriteEffects.FlipHorizontally : SpriteEffects.None, 0f );

                Bitmap image = new Bitmap(texture);

                //g.Clip = new Region(clipRect); //clip region

                ////do rotation (should we do this if angle is 0?)
                float x1 = point.X + ((image.Width * 1f) / 2f);
                float y1 = point.Y + ((image.Height * 1f) / 2f);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.TranslateTransform(-x1, -y1, MatrixOrder.Append);
                g.RotateTransform(Angle, MatrixOrder.Append);
                g.ScaleTransform(1f, 1f, MatrixOrder.Append);
                g.TranslateTransform(x1, y1, MatrixOrder.Append);

                if (Flipped)
                {
                    image.RotateFlip(RotateFlipType.RotateNoneFlipX);
                }

                g.DrawImage(image, point.X, point.Y);
                g.ResetTransform();
            }
        }

        protected Rectangle clipRect;
    }
}