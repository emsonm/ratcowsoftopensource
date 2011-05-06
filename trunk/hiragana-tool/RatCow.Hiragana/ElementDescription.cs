using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RatCow.Hiragana
{
  internal class ElementDescription
  {
    public ElementDescription(char initial, int offset, int multiplier)
    {
      this.Initial = initial;
      this.Offset = offset;
      this.Multiplier = multiplier;
    }

    public char Initial { get; internal set; }
    public int Offset { get; internal set; }
    public int Multiplier { get; internal set; }
  }

  internal class ElementDescriptionList : List<ElementDescription>
  {

    public void Add(char initial, int offset, int multiplier)
    {
      this.Add(new ElementDescription(initial, offset, multiplier));
    }
  }
}
