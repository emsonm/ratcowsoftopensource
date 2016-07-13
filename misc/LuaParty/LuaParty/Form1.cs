using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using NLua;

namespace LuaParty
{
    public partial class Form1 : Form, IOutput
    {
        LuaNet nobj = new LuaNet();

        public Form1()
        {
            InitializeComponent();

            nobj.StopAction += nobj_StopAction;
        }

        void nobj_StopAction(object sender, EventArgs e)
        {
            timer1.Enabled = false;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var script = String.Format(
                "{0}",
                textBox1.Text
            );

            nobj.SetScript(script);
            nobj.HookOutput(this); //try reverse hooking

            Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        public void Start()
        {
            nobj.Ticks = 0;
            nobj.Call("Start", null);
            timer1.Enabled = true;
        }

        public void Loop()
        {
            nobj.Call("Loop", null);
            nobj.Ticks++;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            Loop();
        }

        public void WriteOut(string value)
        {
            label1.Text += value + "\r\n";
        }
    }
}
