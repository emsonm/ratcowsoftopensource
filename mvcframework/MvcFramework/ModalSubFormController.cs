using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RatCow.MvcFramework
{
  public class ModalSubFormController<T> : BaseController<T>, IModalSubFormContainer
  {

    public ModalSubFormController()
      : base()
    {
    }

    public virtual bool PerformModalTask()
    {
      throw new NotImplementedException();
    }
  }
}
