using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RatCow.Utilities
{
  public static class CastExtensions
  {
    public static T CastTo<T>( this object objectToCast )
    {
      return (T)objectToCast;
    }
  }
}