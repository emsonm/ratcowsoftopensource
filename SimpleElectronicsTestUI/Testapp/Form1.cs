using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Testapp
{
    public partial class Form1 : Form, Testapp.ILcd, Testapp.IGeneral
    {
        Label[,] _lcd = null;

        Point _cursor = new Point(0, 0);

        public Form1()
        {
            InitializeComponent();

            InitControlIds();

            InitLcd();

            RunTest();

            initSketch();
        }

        private void InitControlIds()
        {
            _pins = new Control[] { C1, C2, C3, L1, L2, L3 };
        }

        private void RunTest()
        {
            for (int y = 0; y < 2; y++)
                for (int x = 0; x < 16; x++)
                {
                    SetCursor(x, y);
                    Write('a');
                }

            Clear();
        }

        private void InitLcd()
        {
            _lcd = new Label[,] {
                    { D00, D01, D02, D03, D04, D05, D06, D07, D08, D09, D0A, D0B, D0C, D0D, D0E, D0F },
                    { D10, D11, D12, D13, D14, D15, D16, D17, D18, D19, D1A, D1B, D1C, D1D, D1E, D1F }
            };
        }

        public void SetCursor(int x, int y)
        {
            _cursor.X = x;
            _cursor.Y = y;
        }

        public void Write(char c)
        {
            if (_cursor.X >= 0 && _cursor.X < 16 && _cursor.Y >= 0 && _cursor.Y < 2)
            {
                _lcd[_cursor.Y, _cursor.X].Text = c.ToString();
            }
        }

        public void Clear()
        {
            for (int y = 0; y < 2; y++)
                for (int x = 0; x < 16; x++)
                {
                    SetCursor(x, y);
                    Write('\0');
                }

            _cursor.X = 0;
            _cursor.Y = 0;
        }


        public void Print(string s)
        {
            foreach (char c in s)
            {
                Write(c);
                _cursor.X += 1;
            }
        }

        public void Print(long l)
        {
            Print(l.ToString());
        }

        public void Begin(int x, int h)
        {
            Clear();
        }

        long __timer = 0;

        public long millis()
        {
            return __timer; // (DateTime.Now.Ticks - startupTime);
        }

        LCDTest _sketch = new LCDTest();
        long startupTime = DateTime.Now.Ticks;

        void initSketch()
        {
            _sketch.InitInterface<IGeneral>(this);
            _sketch.InitInterface<ILcd>(this);
            _sketch.setup();
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (_sketch != null)
            {
                __timer += timer1.Interval;
                _sketch.loop();
            }
        }


        public void delay(int time)
        {
            timer1.Enabled = false;
            System.Threading.Thread.Sleep(time);
            timer1.Enabled = true;
        }

        Control[] _pins;
        bool[] _outputFlags = new bool[] {false, false, false, false, false, false};

        public void pinMode(int id, byte mode)
        {
            //this does nothing with the actual data... the pins are fixed in the test harness

            if (mode == 1)
            {
                //_pins[id - 1].Click += delegate(object sender, EventArgs e)
                //{
                //    lock(_outputFlags)
                //    {
                //        _outputFlags[id - 1] = true; // set high
                //    }
                //};

                _pins[id - 1].MouseDown += delegate(object sender, MouseEventArgs e)
                {
                    lock(_outputFlags)
                    {
                        _outputFlags[id - 1] = true; // set high
                    }
                };

                _pins[id - 1].MouseUp += delegate(object sender, MouseEventArgs e)
                {
                    lock (_outputFlags)
                    {
                        _outputFlags[id - 1] = false; // set high
                    }
                };
            }
        }


        public int digitalRead(int id)
        {
            lock (_outputFlags)
            {
                var value = _outputFlags[id - 1] ? 1 : 0;
                return value;
            }            
        }

        public void digitalWrite(int id, int value)
        {
            _pins[id - 1].Visible = value == 1;
        }
    }
}
