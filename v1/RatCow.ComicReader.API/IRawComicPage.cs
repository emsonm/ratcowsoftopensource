using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComicReader.API
{
  public interface IRawComicPage<T>
  {
    string Path { get; set; }
    T Entry { get; set; }
  }
}
