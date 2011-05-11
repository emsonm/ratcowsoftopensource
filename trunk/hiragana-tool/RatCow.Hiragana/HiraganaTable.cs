/*
 * Copyright 2011 Rat Cow Software and Matt Emson. All rights reserved.
 * 
 * Redistribution and use in source and binary forms, with or without modification, are
 * permitted provided that the following conditions are met:
 * 
 * 1. Redistributions of source code must retain the above copyright notice, this list of
 *    conditions and the following disclaimer.
 * 2. Redistributions in binary form must reproduce the above copyright notice, this list
 *    of conditions and the following disclaimer in the documentation and/or other materials
 *    provided with the distribution.
 * 3. Neither the name of the Rat Cow Software nor the names of its contributors may be used 
 *    to endorse or promote products derived from this software without specific prior written 
 *    permission.
 *    
 * THIS SOFTWARE IS PROVIDED BY RAT COW SOFTWARE "AS IS" AND ANY EXPRESS OR IMPLIED
 * WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND
 * FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT SHALL <COPYRIGHT HOLDER> OR
 * CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON
 * ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING
 * NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF
 * ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 *
 * The views and conclusions contained in the software and documentation are those of the
 * authors and should not be interpreted as representing official policies, either expressed
 * or implied, of Rat Cow Software and Matt Emson.
 * 
 */


using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RatCow.Hiragana
{
  using KanaSupport;

  //some might argue that is would have been simpler to hard code this...
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

          else if (d.Initial == 't' || d.Initial == 'd')
          {
            if (i == 1 && d.Initial == 't')
            {
              fHiriganaList.Add(
              new Kana(
                d.Offset + (i * d.Multiplier),
                "chi"
                )
               );
            }
            if (i == 1 && d.Initial == 'd')
            {
              fHiriganaList.Add(
              new Kana(
                d.Offset + (i * d.Multiplier),
                "ji"
                )
               );
            }
            else if (i == 2 && d.Initial == 't')
            {
              fHiriganaList.Add(
              new Kana(
                d.Offset + (i * d.Multiplier) +1, //stupid unicode!
                "tsu"
                )
               );
            }
            else if (i == 2 && d.Initial == 'd')
            {
              fHiriganaList.Add(
              new Kana(
                d.Offset + (i * d.Multiplier) + 1, //stupid unicode!
                "zu"
                )
               );
            }
            else
            {
              int hedge = (i > 2 ? 1 : 0);

              fHiriganaList.Add(
                new Kana(
                  d.Offset + (i * d.Multiplier) + hedge,
                  String.Format("{0}{1}", d.Initial, vowels[i])
                  )
                 );
            }
          }

          #region regular Unicode rule

          else
          {
            #region shi chi ji rule

            if (i == 1)
            {
              if (d.Initial == 's')
              {
                fHiriganaList.Add(
                new Kana(
                  d.Offset + (i * d.Multiplier),
                  "shi"
                  )
                 );
              }
              else if (d.Initial == 'z')
              {
                fHiriganaList.Add(
                new Kana(
                  d.Offset + (i * d.Multiplier),
                  "ji"
                  )
                 );
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

    public string EncodeBlock(char kana)
    {
      //
      Kana result = fHiriganaList.FirstOrDefault(k => k.Character == kana);

      string s = null;

      if (result != null)
        s = result.Label;

      return s;
    }

  }
}
