using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace RatCow.MvcFramework.Tools
{
  public class ViewActionMap
  {
    public ViewActionMap()
    {
      GlobalMap = new ViewActions();
      ControlActionMap = new ViewControlActions();
    }

    /// <summary>
    /// Simplify getting the defaults
    /// </summary>
    public ViewActionMap(bool createDefaults)
      : this()
    {
      if (createDefaults) InitDefaults();
    }

    //global action list
    [XmlArray("GlobalMap")]
    [XmlArrayItem("ViewAction")]
    public ViewActions GlobalMap { get; set; }

    //specific controls (this augments, never overrides)
    [XmlArray("ControlActionMap")]
    [XmlArrayItem("ViewControlAction")]
    public ViewControlActions ControlActionMap { get; set; }

    /// <summary>
    /// This is called to create a default file
    /// </summary>
    public virtual void InitDefaults()
    {
      GlobalMap.Add(new ViewAction() { EventName = "Click", EventHandlerName = "EventHandler", EventArgsName = "EventArgs" });

      var textBoxAction = new ViewActions();
      textBoxAction.Add(new ViewAction() { EventName = "TextChanged", EventHandlerName = "EventHandler", EventArgsName = "EventArgs" });
      ControlActionMap.Add(new ViewControlAction() { ControlType = "TextBox", ControlActions = textBoxAction });

      var comboBoxAction = new ViewActions();
      comboBoxAction.Add(new ViewAction() { EventName = "CheckedChanged", EventHandlerName = "EventHandler", EventArgsName = "EventArgs" });
      ControlActionMap.Add(new ViewControlAction() { ControlType = "CheckBox", ControlActions = comboBoxAction });
    }

    /// <summary>
    /// Utility function - this should be moved to a factory
    /// </summary>
    public static ViewActionMap Load(string filename)
    {
      XmlSerializer xmlSerializer = new XmlSerializer(typeof(ViewActionMap));

      ViewActionMap data = null;

      try
      {
        using (var reader = new StreamReader(new FileStream(filename, FileMode.Open, FileAccess.Read)))
        {
          data = (ViewActionMap)xmlSerializer.Deserialize(reader);
        }
      }
      catch (Exception ex) //should specialise this.
      {
        System.Diagnostics.Debug.WriteLine(ex.Message);
        data = null; //just to be sure
      }

      return data;
    }

    /// <summary>
    /// Utility function - this should be moved to a factory
    /// </summary>
    public static bool Save(ViewActionMap data, string filename, bool overwrite)
    {
      var exists = File.Exists(filename);

      if (exists)
      {
        if (overwrite) File.Delete(filename);
        else return false;
      }

      XmlSerializer xmlSerializer = new XmlSerializer(typeof(ViewActionMap));

      using (StreamWriter writer = System.IO.File.CreateText(filename))
      {
        xmlSerializer.Serialize(writer, data);
      }

      return true;
    }
  }
}