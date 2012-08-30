using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using RatCow.MvcFramework.Mapping;

namespace testapp3
{
  public class Data
  {
    [MappedValue("textBox1")]
    public string SomeText { get; set; }
  }
}