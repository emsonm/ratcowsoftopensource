using System;
using System.Drawing;

namespace sharppunk
{
    public class Graphic
    {
        public Boolean Active = true;
        public Boolean Visible = true;

        public Graphic()
        {
        }

        public virtual void Update()
        {
            // to override
        }

        public virtual void Render(Graphics target, Vector2 point, Vector2 camera)
        {
            // to override
        }
    }
}