using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestCapacha
{
  public partial class Form1 : Form
  {
    public Form1()
    {
      InitializeComponent();
    }

    RatCow.Utilities.CaptchaImage capatcha = new RatCow.Utilities.CaptchaImage();
    string legend = String.Empty;

    private void Form1_Load( object sender, EventArgs e )
    {
      Generate();
    }

    private void Generate()
    {
      capatcha.Refresh();
      legend = capatcha.Text;
      pictureBox1.Image = capatcha.RenderImage();
    }

    private void button1_Click( object sender, EventArgs e )
    {
      if ( String.Compare( textBox1.Text, legend, true ) == 0 )
      {
        MessageBox.Show( "Match!" );
        Generate();
      }
      else MessageBox.Show( "Failed!" );
    }
  }
}