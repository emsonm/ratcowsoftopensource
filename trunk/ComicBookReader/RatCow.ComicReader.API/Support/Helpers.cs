using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RatCow.ComicReader.API
{
  public static class StringHelper
  {
    /// <summary>
    /// Remove the quotes if they exist
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string Unquote(this string s)
    {
      if (s.StartsWith("\"") && s.EndsWith("\""))
        return s.Trim('\\', '\"');
      else
        return s;      
    }

    /// <summary>
    /// add quotes
    /// </summary>
    /// <param name="s"></param>
    /// <returns></returns>
    public static string Quote(this string s)
    {
      return String.Format("\"{0}\"", s);
    }
  }
}
