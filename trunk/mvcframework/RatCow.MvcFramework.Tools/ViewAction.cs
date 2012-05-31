using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using System.Xml;
using System.Xml.Serialization;

namespace RatCow.MvcFramework.Tools
{
  public class ViewAction
  {
    public ViewAction()
    {
    }

    public string EventName { get; set; }

    public string EventHandlerName { get; set; }

    public string EventArgsName { get; set; }
  }

  public class ViewControlAction
  {
    public ViewControlAction()
    {
      ControlActions = new ViewActions();
    }

    public string ControlType { get; set; }

    [XmlArray("ControlActions")]
    [XmlArrayItem("ViewAction")]
    public ViewActions ControlActions { get; set; }
  }
}