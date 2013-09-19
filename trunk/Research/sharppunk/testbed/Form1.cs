using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace testbed
{
    public partial class Form1 : Form
    {
        private Bitmap buffer;

        public Form1()
        {
            InitializeComponent();

            buffer = new Bitmap(pictureBox1.Width, pictureBox1.Height);

            pictureBox1.Image = buffer;

            image = Bitmap.FromFile("test.bmp");
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            pictureBox1.Invalidate();
        }

        private int x = 0, y = 0;
        private int direction = 10;
        private float rotation = 0;
        private Image image;

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            using (var g = Graphics.FromImage(buffer))
            {
                g.Clear(Color.AliceBlue);
                g.DrawRectangle(new Pen(Color.Black), x, y, 50, 50);

                x += direction;
                if (x + 25 < 0)
                {
                    direction = 10;
                    y = y + 50;
                }
                if (x - 25 > buffer.Width)
                    direction = -10;

                //test the rotation
                float x1 = 100 + ((image.Width * 1f) / 2f);
                float y1 = 100 + ((image.Height * 1f) / 2f);
                g.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.HighQuality;
                g.TranslateTransform(-x1, -y1, MatrixOrder.Append);
                g.RotateTransform(rotation, MatrixOrder.Append);
                g.ScaleTransform(1f, 1f, MatrixOrder.Append);
                g.TranslateTransform(x1, y1, MatrixOrder.Append);
                g.DrawImage(image, 100, 100);
                g.ResetTransform();

                rotation += 15;
                if (rotation > 360) rotation = 0;
            }
        }
    }
}