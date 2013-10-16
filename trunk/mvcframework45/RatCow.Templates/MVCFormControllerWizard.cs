using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;

namespace RatCow.Templates
{
  public class MVCFormControllerWizard : BaseWizard
  {
    /// <summary>
    /// This implements the elements we need for the MVCFormController and related Priject Items
    /// </summary>
    /// <param name="item"></param>
    /// <returns></returns>
    protected override ProjectItemTypes GetProjectItemType( ProjectItem item )
    {
      if ( item.Name.ToLower().IndexOf( ".mvcmap" ) > 0 )
      {
        return ProjectItemTypes.Child;
      }
      if ( item.Name.ToLower().IndexOf( ".resx" ) > 0 )
      {
        return ProjectItemTypes.Child;
      }
      if ( item.Name.ToLower().IndexOf( ".designer.cs" ) > 0 )
      {
        return ProjectItemTypes.Child;
      }

      return ProjectItemTypes.Parent;
    }
  }
}
