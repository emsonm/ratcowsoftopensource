using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace testapp3
{
  partial class DataProxy : RatCow.MvcFramework.Mapping.DataProxy
  {
    public event EventHandler DataChanged;

    public override void ValueWasModified(object sender, EventArgs e)
    {
      if (DataChanged != null)
        DataChanged(sender, e);
    }
  }
}