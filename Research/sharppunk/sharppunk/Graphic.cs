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

        virtual public void Update()
        {
            // to override
        }

        virtual public void Render(Graphics target, Vector2 point, Vector2 camera)
        {
            // to override
        }
    }
}