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

namespace RatCow.ComicReader.API
{
  /// <summary>
  /// This class reads comic book files
  /// </summary>
  public class ComicBookReader : IEnumerable<IComicPage>
  {

    ILowLevelFileAccess ffile = null;

    public IComicBook Owner { get; set; }

    internal static ComicBookFileType GetFileType(string fileName)
    {
      ComicBookFileType fileType = ComicBookFileType.UNKNOWN;

      //by file extension
      string ext = Path.GetExtension(fileName).ToLower();
      if (ext == ".rar" || ext == ".cbr")
      {
        fileType = ComicBookFileType.CBR;
      }
      else if (ext == ".zip" || ext == ".cbz")
      {
        fileType = ComicBookFileType.CBZ;
      }

      return fileType;
    }

    public ComicBookReader(string fileName)
      : this (fileName, GetFileType(fileName))
    {
    
    }

    /// <summary>
    /// We open the comicbook
    /// </summary>
    /// <param name="fileName"></param>
    public ComicBookReader(string fileName, ComicBookFileType fileType)
    {
      switch (fileType)
      {
        case ComicBookFileType.CBR:
          OpenRAR(fileName);
          break;
        case ComicBookFileType.CBZ:
          OpenZIP(fileName);
          break;
        default:
          throw new FieldAccessException("Unknown file type is not supported.");
      }
    }

    void OpenRAR(string fileName)
    {
      ffile = new RarFileAccess();
      ffile.Init(fileName);
    }

    void OpenZIP(string fileName)
    {
      ffile = new ZipFileAccess();
      ffile.Init(fileName);
    }

    public IComicPage this[int index] { get { return ffile.GetPage(index); } }

    //How many pages we have
    public int PageCount { get { return ffile.PageCount; } }


    #region IEnumerable<IComicPage> Members

    public IEnumerator<IComicPage> GetEnumerator()
    {
      for (int i = 0; i < ffile.PageCount; i++)
      {
        IComicPage page = ffile.GetPage(i);
        page.SetOwner(Owner);
        yield return page;
      }
    }

    #endregion

    #region IEnumerable Members

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
      for (int i = 0; i < ffile.PageCount; i++)
      {
        IComicPage page = ffile.GetPage(i);
        page.SetOwner(Owner);
        yield return page;
      }
    }

    #endregion

    internal int GetIndexForPage(IComicPage page)
    {
      int index = ffile.GetPageIndex(page);
      return index;
    }
  }
}
