using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RatCow.MvcFramework.Tools
{

  public class ViewAssemblies : List<ViewAssembly>
  {
    public ViewAssemblies()
    {

    }

  }

  public class ViewAssembly
  {
    public ViewAssembly()
    {
    }

    public string AssemblyName { get; set; }
    public string AssemblyFullName {get; set;}
    public string HintPath { get; set; }
  }
}
