using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RatCow.Hiragana
{
  public class HiraganaTable
  {
    public HiraganaTable()
    {
      Init();
    }

    List<Kana> fHiriganaList = new List<Kana>();

    //the Unicode encoding rules.. NB - the W- and final -N are not included here
    ElementDescriptionList descriptions = new ElementDescriptionList {
     {' ', 12354, 2}, //stand alone
     {'k', 12363, 2},
     {'g', 12364, 2}, //"
     {'s', 12373, 2},
     {'z', 12374, 2}, //"
     {'t', 12383, 2},
     {'d', 12384, 2}, //"
     {'n', 12394, 1},
     {'h', 12399, 3},
     {'b', 12400, 3}, //"
     {'p', 12401, 3}, //o
     {'m', 12414, 1},
     {'y', 12420, 1},  //only a u o is included
     {'r', 12425, 1} 
     //{'w', 12431, 1}  //removed as it breaks the above rules!!
    };

    char[] vowels = { 'a', 'i', 'u', 'e', 'o' };

    //we create the table - this is a little ugly
    internal void Init()
    {
      foreach (ElementDescription d in descriptions)
      {
        for (int i = 0; i < 5; i++)
        {
          #region stand alone Vowels

          if (d.Initial == ' ')
          {
            fHiriganaList.Add(
              new Kana(
                d.Offset + (i * d.Multiplier),
                String.Format("{0}", vowels[i])
                )
               );
          }

          #endregion

          #region Rule for Y-

          else if (d.Initial == 'y')
          {
            //special rule
            if (i == 1 | i == 3)
            {
              continue; //skip not used chars
            }
            else
            {
              fHiriganaList.Add(
                new Kana(
                  d.Offset + (i * d.Multiplier),
                  String.Format("{0}{1}", d.Initial, vowels[i])
                  )
                 );
            }
          }

          #endregion

          #region regular Unicode rule

          else
          {
            fHiriganaList.Add(
              new Kana(
                d.Offset + (i * d.Multiplier),
                String.Format("{0}{1}", d.Initial, vowels[i])
                )
               );
          }

          #endregion
        }
      }

      //sort of hard coded, I know
      #region special case W-
      fHiriganaList.Add(new Kana(12431, "wa"));
      fHiriganaList.Add(new Kana(12434, "wo"));
      #endregion

      #region final -N

      fHiriganaList.Add(new Kana(12435, "n"));

      #endregion
    }

    #region Debug code

    // Simple test routine - generates a Hiragana table from Unicode characters
    public string Test()
    {
      StringBuilder result = new StringBuilder();

      int counter = 0;
      foreach (Kana k in fHiriganaList)
      {
        result.AppendFormat("{0} - {1}, ", k.Character, k.Label);
        counter++;
        if (counter >= 5 || (counter == 3 && k.Label == "yo") || (counter == 2 && k.Label == "wo"))
        {
          counter = 0;
          result.AppendLine();
        }
      }

      return result.ToString();
    }

    #endregion

    public char DecodeBlock(string block)
    {
      //
      Kana result = fHiriganaList.FirstOrDefault(k => k.Label == block);

      char c = (char)32;

      if (result != null)
        c = result.Character;

      return c;
    }

  }
}
