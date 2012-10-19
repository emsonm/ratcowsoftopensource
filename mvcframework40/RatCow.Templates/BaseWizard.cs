
/* This code is based on the Code Project article found here:
 * 
 * http://www.codeproject.com/Articles/23182/Create-Item-Templates-Which-Have-Nested-Items  
 * 
 * The author of this article was Sarafian, http://www.codeproject.com/Members/Sarafian (http://sarafianalex.wordpress.com/)
 * 
 * This code is licensed under the Code Project Open license : http://www.codeproject.com/info/cpol10.aspx,
 * a copy of which may be downloaded from this link: http://www.codeproject.com/info/CPOL.zip
 */

/*
 * Copyright 2003 - 2012 Sarafian, with modifications by Rat Cow Software and Matt Emson. 
 * All rights reserved.
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
 * or implied, of Rat Cow Software, Matt Emson and any other individual sited in the copyright 
 * statement above.
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using EnvDTE;
using Microsoft.VisualStudio.TemplateWizard;

using System.IO;

namespace RatCow.Templates
{
  public abstract class BaseWizard : IWizard
  {
    public void BeforeOpeningFile( ProjectItem projectItem )
    {
    }

    public void ProjectFinishedGenerating( Project project )
    {
    }

    string GetBaseName( string name )
    {
      var index = name.IndexOf( "." ); //you could use "LastIndexOf()" here, but that breaks some file logic. We are sticking with "A parent has the name Xxxx.cs" here. If you don't like that, change away!
      if ( index > -1 )
        return name.Substring( 0, index );
      else
        return name;
    }

    /// <summary>
    /// No longer assumes one parent item
    /// </summary>
    public void ProjectItemFinishedGenerating( ProjectItem projectItem )
    {
      ProjectItemTypes type = GetProjectItemType( projectItem );
      switch ( type )
      {
        case ProjectItemTypes.Parent:
          {
            var name = GetBaseName( projectItem.Name );
          
            this.fParentProjectItems.Add( name, projectItem );
            break;
          }
        case ProjectItemTypes.Child:
          this.fChildProjectItems.Add( projectItem );
          break;
      }
    }

    /// <summary>
    /// This now accounts for project items with more than one parent type
    /// </summary>
    public void RunFinished()
    {
      foreach ( ProjectItem item in this.fChildProjectItems )
      {
        string filename = item.get_FileNames( 0 );

        var name = GetBaseName( Path.GetFileName(filename) );

        ProjectItem selectedProjectItem;

        if ( fParentProjectItems.TryGetValue( name, out selectedProjectItem ) )
        {
          selectedProjectItem.ProjectItems.AddFromFile( filename );
        }
      }
    }

    public void RunStarted( object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams )
    {
    }

    public bool ShouldAddProjectItem( string filePath )
    {
      return true;
    }

    /// <summary>
    /// Controls where we put the items we are about to add
    /// </summary>
    protected abstract ProjectItemTypes GetProjectItemType( ProjectItem item );

    private Dictionary<String, ProjectItem> fParentProjectItems = new Dictionary<string, ProjectItem>();
    private List<ProjectItem> fChildProjectItems = new List<ProjectItem>();



    protected enum ProjectItemTypes { Parent, Child };
  }
}
