using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace CFTest2003
{
  internal class MainFormController : AbstractMainFormController
  {
    protected override void button1Click()
    {
      textBox1.Text = "Hello, world";
    }

    [RatCow.MvcFramework.Action("textBox1", "KeyDown")]
    public void textBox1_KeyDown(object sender, System.Windows.Forms.KeyEventArgs e)
    {
      if (e.KeyCode == System.Windows.Forms.Keys.F1)
      {
        textBox1.Text = "BINGO!!";
        e.Handled = true;
      }
    }

  }
}
