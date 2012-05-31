using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RatCow.MvcFramework.Tools
{
  public class CompilerFlags
  {
    public bool IsAbstract = false;
    public bool UsePartialMethods = false;
    public bool PassControllerToEvents = false;
    public bool ProtectListViews = false;
    public bool RestrictActions = false;
    public bool UseDefaultActionsFile = false;
  }
}