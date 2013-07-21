using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace RatCow.TestBed
{
    using RatCow.Controls;

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RatCow.Controls.GDIPlusGraphicsContext.GraphicContext.Init();

            var image = new Bitmap(this.Width - 20, this.Height - 20);

            GraphicContext.Instance.NativeTargetObject = image;

            pictureBox1.Image = image;


            var button1 = new Button() { Top = 30, Left = 30, Height = 35, Width = 50, Text = "test", Enabled = true, Visible = true, Focused = false, Name = "Button1" };
            button1.Click += new ControlAction(button1_Click);

            var textBox1 = new TextBox() { Top = 100, Left = 100, Height = 35, Width = 100, Enabled = true, Visible = true, Focused = false, Selected = true };

            var cbx1 = new CheckBox() { Top = 150, Left = 30, Height = 35, Width = 50, Text = "test", Enabled = true, Visible = true, Focused = false, Name = "Button1" };

            GraphicContext.Instance.GraphicsObjects.Add(button1);
            GraphicContext.Instance.GraphicsObjects.Add(textBox1);
            GraphicContext.Instance.GraphicsObjects.Add(cbx1);

        }

        void button1_Click(object sender)
        {
            //MessageBox.Show("!!");
        }

        private void Form1_Paint(object sender, PaintEventArgs e)
        {
            var mouse = pictureBox1.PointToClient(System.Windows.Forms.Control.MousePosition);
            GraphicContext.Instance.Render(mouse.X, mouse.Y, null);
            pictureBox1.Image = GraphicContext.Instance.NativeTargetObject as Bitmap;
        }

        private void Form1_MouseMove(object sender, MouseEventArgs e)
        {
            var mouse = pictureBox1.PointToClient(System.Windows.Forms.Control.MousePosition);
            GraphicContext.Instance.Render(mouse.X, mouse.Y, null);
            pictureBox1.Image = GraphicContext.Instance.NativeTargetObject as Bitmap;
        }

        private void Form1_MouseDown(object sender, MouseEventArgs e)
        {
            var mouse = pictureBox1.PointToClient(System.Windows.Forms.Control.MousePosition);
            GraphicContext.Instance.Render(mouse.X, mouse.Y, true);
            pictureBox1.Image = GraphicContext.Instance.NativeTargetObject as Bitmap;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            var mouse = pictureBox1.PointToClient(System.Windows.Forms.Control.MousePosition);
            GraphicContext.Instance.Render(mouse.X, mouse.Y, false);
            pictureBox1.Image = GraphicContext.Instance.NativeTargetObject as Bitmap;
        }

        private void Form1_KeyPress(object sender, KeyPressEventArgs e)
        {           
            GraphicContext.Instance.RenderText(e.KeyChar);
            pictureBox1.Image = GraphicContext.Instance.NativeTargetObject as Bitmap;
        }

        private void Form1_KeyDown(object sender, KeyEventArgs e)
        {
            Key key = TranslateKey(e.KeyCode);

            if (key != Key.Unmapped)
            {
                GraphicContext.Instance.RenderKey(key, e.Shift, e.Control, e.Alt);
                pictureBox1.Image = GraphicContext.Instance.NativeTargetObject as Bitmap;
            }
        }

        private Key TranslateKey(Keys keys)
        {
            switch (keys)
            {
                case Keys.Delete:
                    return Key.Delete;
                case Keys.Back:
                    return Key.BackSpace;
                case Keys.Left:
                    return Key.Left;
                case Keys.Right:
                    return Key.Right;
                default:
                    return Key.Unmapped;
            }
        }
    }
}
