using System;
using System.Collections.Generic;
//using System.Linq;
using System.Text;

namespace CFTest2003
{
  internal class MainFormController: AbstractMainFormController
  {
      protected override void button1Click()
      {
          textBox1.Text = "Hello, world";
      }
  }
}
