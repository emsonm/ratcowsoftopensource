using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RatCow.MvcFramework.Tools
{
  public class ControlTree
  {
    public string ClassName { get; set; }
    public string NamespaceName { get; set; }

    public Dictionary<string, Type> Controls = new Dictionary<string, Type>();
    public void AddControl(string name, Type type)
    {
      Controls.Add(name, type);
    }

  }
}
