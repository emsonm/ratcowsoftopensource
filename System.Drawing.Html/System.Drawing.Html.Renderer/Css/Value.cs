using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing.Drawing2D;
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Globalization;

namespace System.Drawing.Html.Renderer.Css
{
  public static class Value
  {
    /// <summary>
    /// Evals a number and returns it. If number is a percentage, it will be multiplied by <see cref="hundredPercent"/>
    /// </summary>
    /// <param name="number">Number to be parsed</param>
    /// <param name="factor">Number that represents the 100% if parsed number is a percentage</param>
    /// <returns>Parsed number. Zero if error while parsing.</returns>
    public static float ParseNumber(string number, float hundredPercent)
    {
      if (string.IsNullOrEmpty(number))
      {
        return 0f;
      }

      string toParse = number;
      bool isPercent = number.EndsWith("%");
      float result = 0f;

      if (isPercent) toParse = number.Substring(0, number.Length - 1);

      if (!float.TryParse(toParse, NumberStyles.Number, NumberFormatInfo.InvariantInfo, out result))
      {
        return 0f;
      }

      if (isPercent)
      {
        result = (result / 100f) * hundredPercent;
      }

      return result;
    }

    /// <summary>
    /// Parses a length. Lengths are followed by an unit identifier (e.g. 10px, 3.1em)
    /// </summary>
    /// <param name="length">Specified length</param>
    /// <param name="hundredPercent">Equivalent to 100 percent when length is percentage</param>
    /// <param name="box"></param>
    /// <returns></returns>
    public static float ParseLength(string length, float hundredPercent, Box box)
    {
      return ParseLength(length, hundredPercent, box, box.GetEmHeight(), false);
    }

    /// <summary>
    /// Parses a length. Lengths are followed by an unit identifier (e.g. 10px, 3.1em)
    /// </summary>
    /// <param name="length">Specified length</param>
    /// <param name="hundredPercent">Equivalent to 100 percent when length is percentage</param>
    /// <param name="box"></param>
    /// <param name="useParentsEm"></param>
    /// <param name="returnPoints">Allows the return float to be in points. If false, result will be pixels</param>
    /// <returns></returns>
    public static float ParseLength(string length, float hundredPercent, Box box, float emFactor, bool returnPoints)
    {
      //Return zero if no length specified, zero specified
      if (string.IsNullOrEmpty(length) || length == "0") return 0f;

      //If percentage, use ParseNumber
      if (length.EndsWith("%")) return ParseNumber(length, hundredPercent);

      //If no units, return zero
      if (length.Length < 3) return 0f;

      //Get units of the length
      string unit = length.Substring(length.Length - 2, 2);

      //Factor will depend on the unit
      float factor = 1f;

      //Number of the length
      string number = length.Substring(0, length.Length - 2);

      //TODO: Units behave different in paper and in screen!
      switch (unit)
      {
        case Constants.Em:
          factor = emFactor;
          break;
        case Constants.Px:
          factor = 1f;
          break;
        case Constants.Mm:
          factor = 3f; //3 pixels per millimeter
          break;
        case Constants.Cm:
          factor = 37f; //37 pixels per centimeter
          break;
        case Constants.In:
          factor = 96f; //96 pixels per inch
          break;
        case Constants.Pt:
          factor = 96f / 72f; // 1 point = 1/72 of inch

          if (returnPoints)
          {
            return ParseNumber(number, hundredPercent);
          }

          break;
        case Constants.Pc:
          factor = 96f / 72f * 12f; // 1 pica = 12 points
          break;
        default:
          factor = 0f;
          break;
      }



      return factor * ParseNumber(number, hundredPercent);
    }

    /// <summary>
    /// Parses a color value in CSS style; e.g. #ff0000, red, rgb(255,0,0), rgb(100%, 0, 0)
    /// </summary>
    /// <param name="colorValue">Specified color value; e.g. #ff0000, red, rgb(255,0,0), rgb(100%, 0, 0)</param>
    /// <returns>System.Drawing.Color value</returns>
    public static Color GetActualColor(string colorValue)
    {
      int r = 0;
      int g = 0;
      int b = 0;
      Color onError = Color.Empty;

      if (string.IsNullOrEmpty(colorValue)) return onError;

      colorValue = colorValue.ToLower().Trim();

      if (colorValue.StartsWith("#"))
      {
        #region hexadecimal forms
        string hex = colorValue.Substring(1);

        if (hex.Length == 6)
        {
          r = int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
          g = int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
          b = int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
        }
        else if (hex.Length == 3)
        {
          r = int.Parse(new String(hex.Substring(0, 1)[0], 2), System.Globalization.NumberStyles.HexNumber);
          g = int.Parse(new String(hex.Substring(1, 1)[0], 2), System.Globalization.NumberStyles.HexNumber);
          b = int.Parse(new String(hex.Substring(2, 1)[0], 2), System.Globalization.NumberStyles.HexNumber);
        }
        else
        {
          return onError;
        }
        #endregion
      }
      else if (colorValue.StartsWith("rgb(") && colorValue.EndsWith(")"))
      {
        #region RGB forms

        string rgb = colorValue.Substring(4, colorValue.Length - 5);
        string[] chunks = rgb.Split(',');

        if (chunks.Length == 3)
        {
          unchecked
          {
            r = Convert.ToInt32(ParseNumber(chunks[0].Trim(), 255f));
            g = Convert.ToInt32(ParseNumber(chunks[1].Trim(), 255f));
            b = Convert.ToInt32(ParseNumber(chunks[2].Trim(), 255f));
          }
        }
        else
        {
          return onError;
        }

        #endregion
      }
      else
      {
        #region Color Constants

        string hex = string.Empty;

        switch (colorValue)
        {
          case Constants.Maroon:
            hex = "#800000"; break;
          case Constants.Red:
            hex = "#ff0000"; break;
          case Constants.Orange:
            hex = "#ffA500"; break;
          case Constants.Olive:
            hex = "#808000"; break;
          case Constants.Purple:
            hex = "#800080"; break;
          case Constants.Fuchsia:
            hex = "#ff00ff"; break;
          case Constants.White:
            hex = "#ffffff"; break;
          case Constants.Lime:
            hex = "#00ff00"; break;
          case Constants.Green:
            hex = "#008000"; break;
          case Constants.Navy:
            hex = "#000080"; break;
          case Constants.Blue:
            hex = "#0000ff"; break;
          case Constants.Aqua:
            hex = "#00ffff"; break;
          case Constants.Teal:
            hex = "#008080"; break;
          case Constants.Black:
            hex = "#000000"; break;
          case Constants.Silver:
            hex = "#c0c0c0"; break;
          case Constants.Gray:
            hex = "#808080"; break;
          case Constants.Yellow:
            hex = "#FFFF00"; break;
        }

        if (string.IsNullOrEmpty(hex))
        {
          return onError;
        }
        else
        {
          Color c = GetActualColor(hex);
          r = c.R;
          g = c.G;
          b = c.B;
        }

        #endregion
      }

      return Color.FromArgb(r, g, b);
    }

    /// <summary>
    /// Parses a border value in CSS style; e.g. 1px, 1, thin, thick, medium
    /// </summary>
    /// <param name="borderValue"></param>
    /// <returns></returns>
    public static float GetActualBorderWidth(string borderValue, Box b)
    {
      if (string.IsNullOrEmpty(borderValue))
      {
        return GetActualBorderWidth(Constants.Medium, b);
      }

      switch (borderValue)
      {
        case Constants.Thin:
          return 1f;
        case Constants.Medium:
          return 2f;
        case Constants.Thick:
          return 4f;
        default:
          return Math.Abs(ParseLength(borderValue, 1, b));
      }
    }

    /// <summary>
    /// Split the value by spaces; e.g. Useful in values like 'padding:5 4 3 inherit'
    /// </summary>
    /// <param name="value">Value to be splitted</param>
    /// <returns>Splitted and trimmed values</returns>
    public static string[] SplitValues(string value)
    {
      return SplitValues(value, ' ');
    }

    /// <summary>
    /// Split the value by the specified separator; e.g. Useful in values like 'padding:5 4 3 inherit'
    /// </summary>
    /// <param name="value">Value to be splitted</param>
    /// <returns>Splitted and trimmed values</returns>
    public static string[] SplitValues(string value, char separator)
    {
      //TODO: CRITICAL! Don't split values on parenthesis (like rgb(0, 0, 0)) or quotes ("strings")


      if (string.IsNullOrEmpty(value)) return new string[] { };

      string[] values = value.Split(separator);
      List<string> result = new List<string>();

      for (int i = 0; i < values.Length; i++)
      {
        string val = values[i].Trim();

        if (!string.IsNullOrEmpty(val))
        {
          result.Add(val);
        }
      }

      return result.ToArray();
    }

    /// <summary>
    /// Detects the type name in a path. 
    /// E.g. Gets System.Drawing.Graphics from a path like System.Drawing.Graphics.Clear
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    private static Type GetTypeInfo(string path, ref string moreInfo)
    {
      int lastDot = path.LastIndexOf('.');

      if (lastDot < 0) return null;

      string type = path.Substring(0, lastDot);
      moreInfo = path.Substring(lastDot + 1);
      moreInfo = moreInfo.Replace("(", string.Empty).Replace(")", string.Empty);


      foreach (Assembly a in HtmlRenderer.References)
      {
        Type t = a.GetType(type, false, true);

        if (t != null) return t;
      }

      return null;
    }

    /// <summary>
    /// Returns the object specific to the path
    /// </summary>
    /// <param name="path"></param>
    /// <returns>One of the following possible objects: FileInfo, MethodInfo, PropertyInfo</returns>
    private static bool DetectSource(string path, out object source, out object instance)
    {
      instance = null;
      source = null;

      if (path.StartsWith("method:", StringComparison.CurrentCultureIgnoreCase))   //legacy
      {
        string methodName = string.Empty;
        Type t = GetTypeInfo(path.Substring(7), ref methodName); 
        if (t == null) return false;
        MethodInfo method = t.GetMethod(methodName);

        if (!method.IsStatic || method.GetParameters().Length > 0)
        {
          return false;
        }

        source = method;
        return true;
      }
      else if (path.StartsWith("smethod:", StringComparison.CurrentCultureIgnoreCase))
      {
        string methodName = string.Empty;
        Type t = GetTypeInfo(path.Substring(8), ref methodName);
        if (t == null) return false;
        MethodInfo method = t.GetMethod(methodName);

        if (!method.IsStatic || method.GetParameters().Length > 0)
        {
          return false;
        }

        source = method;
        return true;
      }
      else if (path.StartsWith("property:", StringComparison.CurrentCultureIgnoreCase)) //legacy                                
      {
        string propName = string.Empty;
        Type t = GetTypeInfo(path.Substring(9), ref propName); 
        if (t == null) return false;
        
        PropertyInfo prop = t.GetProperty(propName);

        source = prop;
        return true;
      }
      else if (path.StartsWith("sproperty:", StringComparison.CurrentCultureIgnoreCase))
      {
        string propName = string.Empty;
        Type t = GetTypeInfo(path.Substring(10), ref propName);
        if (t == null) return false;

        PropertyInfo prop = t.GetProperty(propName);

        source = prop;
        return true;
      }
      else if (path.StartsWith("imethod:", StringComparison.CurrentCultureIgnoreCase))
      {
        string[] paths = path.Split('!');

        if (paths.Length != 2) return false;

        string locatorMethod = paths[0];
        Type t = GetTypeInfo(paths[0].Substring(8), ref locatorMethod); 
        if (t == null) return false ;
        
        MethodInfo method = t.GetMethod(locatorMethod);

        if (!method.IsStatic || method.GetParameters().Length > 1)
        {
          return false;
        }

        string[] info = paths[1].Split('.');

        if (info.Length < 1 | info.Length > 2) return false; //this should be a straight Instance.Method

        string methodName = String.Empty;
        //if we have two parts, the first is the name we pass to the registered method, else we only get one item, so we just habe the method
        if (info.Length == 2)
        {
          instance = method.Invoke(null, new object[] { info[0] }); //info[0] is the registered instance name
          methodName = info[1];
        }
        else
        {
          instance = method.Invoke(null, null);
          methodName = info[0];
        }

        if (instance == null) return false;

        t = instance.GetType();          //t = GetTypeInfo(paths[1].Substring(7), ref methodName); 
        if (t == null) return false;

        method = t.GetMethod(methodName);

        if (method.IsStatic || method.GetParameters().Length > 0)
        {
          return false;
        }

        source = method;

        return true;
      }
      else if (path.StartsWith("iproperty:", StringComparison.CurrentCultureIgnoreCase))
      {
        string[] paths = path.Split('!');

        if (paths.Length != 2) return false;

        string locatorMethod = paths[0];
        Type t = GetTypeInfo(paths[0].Substring(10), ref locatorMethod);
        if (t == null) return false;

        MethodInfo method = t.GetMethod(locatorMethod);

        if (!method.IsStatic || method.GetParameters().Length > 1)
        {
          return false;
        }

        string[] info = paths[1].Split('.');

        if (info.Length < 1 | info.Length > 2) return false; //this should be a straight Instance.Property

        string propName = String.Empty;
        //if we have two parts, the first is the name we pass to the registered method, else we only get one item, so we just habe the method
        if (info.Length == 2)
        {
          instance = method.Invoke(null, new object[] { info[0] }); //info[0] is the registered instance name
          propName = info[1];
        }
        else
        {
          instance = method.Invoke(null, null);
          propName = info[0];
        }

        if (instance == null) return false;

        t = instance.GetType();         
        if (t == null) return false;

        //string propName = string.Empty;
        //Type t = GetTypeInfo(path.Substring(10), ref propName);
        //if (t == null) return false;

        PropertyInfo prop = t.GetProperty(propName);

        source = prop;
        return true;
      }
      else if (Uri.IsWellFormedUriString(path, UriKind.RelativeOrAbsolute))
      {
        source = new Uri(path);
        return true;
      }
      else
      {
        source = new FileInfo(path);
        return true;
      }
    }

    /// <summary>
    /// Gets the image of the specified path
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static Image GetImage(string path)
    {
      object source;
      object instance;
      bool success = DetectSource(path, out source, out instance);

      if (!success) return null;

      FileInfo finfo = source as FileInfo;
      PropertyInfo prop = source as PropertyInfo;
      MethodInfo method = source as MethodInfo;

      try
      {
        if (finfo != null)
        {
          if (!finfo.Exists) return null;

          return Image.FromFile(finfo.FullName);

        }
        else if (prop != null)
        {
          if (!prop.PropertyType.IsSubclassOf(typeof(Image)) && !prop.PropertyType.Equals(typeof(Image))) return null;

          return prop.GetValue(instance, null) as Image;
        }
        else if (method != null)
        {
          if (!method.ReturnType.IsSubclassOf(typeof(Image))) return null;

          //instance will be null for static methods!
          return method.Invoke(instance, null) as Image;
        }
        else
        {
          return null;
        }
      }
      catch
      {
        return new Bitmap(50, 50); //TODO: Return error image
      }
    }

    /// <summary>
    /// Gets the content of the stylesheet specified in the path
    /// </summary>
    /// <param name="path"></param>
    /// <returns></returns>
    public static string GetStyleSheet(string path)
    {
      object source;
      object instance;
      bool success = DetectSource(path, out source, out instance);

      if (!success) return null;

      FileInfo finfo = source as FileInfo;
      PropertyInfo prop = source as PropertyInfo;
      MethodInfo method = source as MethodInfo;

      try
      {
        if (finfo != null)
        {
          if (!finfo.Exists) return null;

          StreamReader sr = new StreamReader(finfo.FullName);
          string result = sr.ReadToEnd();
          sr.Dispose();

          return result;
        }
        else if (prop != null)
        {
          if (!prop.PropertyType.Equals(typeof(string))) return null;

          return prop.GetValue(instance, null) as string;
        }
        else if (method != null)
        {
          if (!method.ReturnType.Equals(typeof(string))) return null;

          //instance will be null for static methods!
          return method.Invoke(instance, null) as string;             
        }
        else
        {
          return string.Empty;
        }
      }
      catch
      {
        return string.Empty;
      }
    }

    /// <summary>
    /// Executes the desired action when the user clicks a link
    /// </summary>
    /// <param name="href"></param>
    public static void GoLink(string href)
    {
      object source;
      object instance;
      bool success = DetectSource(href, out source, out instance);

      if (!success) return;

      FileInfo finfo = source as FileInfo;
      PropertyInfo prop = source as PropertyInfo;
      MethodInfo method = source as MethodInfo;
      Uri uri = source as Uri;

      try
      {
        if (finfo != null || uri != null)
        {
          ProcessStartInfo nfo = new ProcessStartInfo(href);
          nfo.UseShellExecute = true;

          Process.Start(nfo);

        }
        else if (method != null)
        {
          //instance will be null for static methods!
          method.Invoke(instance, null);
        }
        else
        {
          //Nothing to do.
        }
      }
      catch
      {
        throw;
      }
    }
  }
}
