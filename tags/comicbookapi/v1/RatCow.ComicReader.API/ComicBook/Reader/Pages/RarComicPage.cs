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
using System.IO;

using NUnrar;

namespace RatCow.ComicReader.API
{
  public class RarComicPage : IComicPage
  {
    CompressedFile fEntry = null;
    MemoryStream fcachedAccess = null;

    public RarComicPage(CompressedFile entry)
    {
      fEntry = entry;
    }

    public Stream RawData
    {
      get
      {
        //This probably leaks memory, so this sould be cached....
        MemoryStream result = null;

        if (fcachedAccess == null)
        {
          result = new MemoryStream();
          fEntry.WriteUncompressedTo(result);
        }
        else
          result = fcachedAccess;

        result.Seek(0, SeekOrigin.Begin);
        
        return result;
      }
    }

    public ImageFormat PageType
    {
      get
      {
        string extension = System.IO.Path.GetExtension(Filename).ToLower();
        if (extension.StartsWith(".pn"))
          return ImageFormat.PNG;
        else if (extension.StartsWith(".jp"))
          return ImageFormat.JPEG;
        else return ImageFormat.UNKNOWN;
      }
    }

    public string Filename
    {
      get { return fEntry.FilePath; }
    }

    #region IComicPage Members

    internal IComicBook fOwner = null;

    public void Activate()
    {
      //this would let the API know which page was active
    }

    public void SetOwner(IComicBook owner)
    {
      fOwner = owner;
    }

    #endregion
  }
}
