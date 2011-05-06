using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

using RatCow.Hiragana;

namespace RatcowHiriganaTool
{
  public partial class MainForm : Form
  {
    public MainForm()
    {
      InitializeComponent();
    }

    private void button1_Click(object sender, EventArgs e)
    {
      HiraganaTable h = new HiraganaTable();

      textBox1.Text = h.Test(); //builds a table of characters
    }

    private void button2_Click(object sender, EventArgs e)
    {
      HiraganaTable h = new HiraganaTable();

      textBox1.Text = String.Format("{0}{1}{2}", h.DecodeBlock("ta"), h.DecodeBlock("na"), h.DecodeBlock("ka"));
    }
  }
}
