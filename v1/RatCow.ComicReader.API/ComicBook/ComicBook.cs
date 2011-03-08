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

namespace RatCow.ComicReader.API
{
  public class ComicBook : IComicBook, IEnumerable<IComicPage>
  {
    ComicBookReader fReader = null;

    public ComicBook(ComicBookReader reader)
    {
      fReader = reader;
      fReader.Owner = this;
      CurrentIndex = 0;
    }

    #region IComicbook Members

    public IComicPage this[int index]
    {
      get
      {
        return fReader[index];
      }
    }

    public int PageCount
    {
      get { return fReader.PageCount; }
    }

    public int CurrentIndex { get; set; }

    public IComicPage Next()
    {
      CurrentIndex++;
      return Current();
    }

    public IComicPage Prior()
    {
      CurrentIndex--;
      return Current();
    }

    public IComicPage Current()
    {
      return fReader[CurrentIndex];
    }

    public IComicPage First()
    {
      CurrentIndex = 0;
      return Current();
    }

    public IComicPage Last()
    {
      CurrentIndex = (fReader.PageCount - 1);
      return Current();
    }

    #endregion

    #region IEnumerable<IComicPage> Members

    public IEnumerator<IComicPage> GetEnumerator()
    {
      return fReader.GetEnumerator();
    }

    #endregion

    #region IEnumerable Members

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      return fReader.GetEnumerator();
    }

    #endregion

    #region IComicBook Members


    public void ActivatePage(IComicPage page)
    {
      int index = fReader.GetIndexForPage(page);
      CurrentIndex = index;
    }

    #endregion
  }
}
