using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RatCow.Hiragana
{
  public class Kana
  {
    //we use the label for looking up morphems
    // the code is the actual unicode char
    public Kana(int code, string label)
    {
      this.Code = code;
      this.Label = label;
    }

    public int Code { get; internal set; }

    public string Label { get; internal set; }

    //hard cast shortcut for lazy programmers
    public char Character
    {
      get
      {
        return (char)Code;
      }
    }
  }

  public class KanaList : List<Kana>
  {

    public void Add(int code, string label)
    {
      this.Add(new Kana(code, label));
    }
  }
}
