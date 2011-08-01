using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RatCow.MvcFramework
{
  using System.Windows.Forms;

  public static class FormHelper
  {
    public static void SetWaitCursor(this Form target)
    {
      target.Cursor = Cursors.WaitCursor;
    }

    public static void SetDefaultCursor(this Form target)
    {
      target.Cursor = Cursors.Default;
    }
  }
}
