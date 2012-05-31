using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using System.Xml;
using System.Xml.Serialization;

namespace RatCow.MvcFramework.Tools
{
  public class ViewActions : List<ViewAction>
  {
    public ViewActions()
    {
    }
  }

  public class ViewControlActions : List<ViewControlAction>
  {
    public ViewControlActions()
    {
    }
  }
}