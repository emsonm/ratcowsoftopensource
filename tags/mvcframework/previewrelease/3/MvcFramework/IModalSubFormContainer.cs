using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RatCow.MvcFramework
{
  /// <summary>
  /// We implement this
  /// </summary>
  public interface IModalSubFormContainer //: BaseController<System.Windows.Forms.Form>
  {
    /// <summary>
    /// Subclasses override this
    /// </summary>
    /// <returns></returns>
    bool PerformModalTask();

    /// <summary>
    /// Performs a task with associated data
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data"></param>
    /// <returns></returns>
    bool PerformModalTask<T>(T data);
  }
}
