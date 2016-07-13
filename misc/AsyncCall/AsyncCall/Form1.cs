using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace AsyncCall
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string data = "1234";
            string data2 = "5678";
            //new AsyncAction<String>(DoAction).CompleteAsyncAction(this, data);

            //AsyncAction<String>.CallAsync(DoAction, data);

            AsyncCallVB.SyncAsyncAction<String>.CallActionAsync(DoAction, data2);

            AsyncCallVB.SyncAsyncAction<String>.CallAction(DoAction, data);

            MessageBox.Show("Done");
        }

        void DoAction(string data)
        {
            MessageBox.Show(data);
        }
    }
}
