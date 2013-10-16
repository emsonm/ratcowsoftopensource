/*
 * Copyright 2012 Rat Cow Software and Matt Emson. All rights reserved.
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
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

//3rd Party
using RatCow.MvcFramework;

namespace RatCow.MvcFramework.MvcMapTool
{

  using Tools;

  internal class Store
  {
    public string ClassName { get; set; }
    public Type ClassType { get; set; }
  }


  internal partial class AddGeneralItemFormController : BaseController<AddGeneralItemForm>, IModalSubFormContainer
  {


    protected override void ViewLoad()
    {
      base.ViewLoad();

      //load the current controls
      LoadClassData();

      //configure the first drop down
      basedOnCombo.DataSource = registeredTypes;
      basedOnCombo.DisplayMember = "ClassName";
      basedOnCombo.ValueMember = "ClassType";
    }


    List<Store> registeredTypes = new List<Store>();
    List<ViewAction> actions = new List<ViewAction>();

    private void LoadClassData()
    {
      Assembly[] assemblies = AppDomain.CurrentDomain.GetAssemblies();
      foreach ( Assembly a in assemblies )
      {
        foreach ( Type t in a.GetExportedTypes() )
        {
          // Ignore type if not a public class
          if ( !t.IsClass || !t.IsPublic ) continue;

          //skip non controls
          if ( !( t == typeof( Control ) || t.IsSubclassOf( typeof( Control ) ) ) ) continue;

          //register the type...
          try
          {
            if ( registeredTypes.Where( x => x.ClassName == t.FullName ).Count() <= 0 )
              registeredTypes.Add( new Store() { ClassName = t.Name, ClassType = t } );
          }
          catch ( Exception ex )
          {
            System.Diagnostics.Debug.WriteLine( String.Format( "{0} - {1}", ex.GetType().Name, ex.Message ) );
            System.Diagnostics.Debug.WriteLine( ex.StackTrace );
          }
        }
      }
    }

    partial void basedOnComboSelectedIndexChanged( EventArgs e )
    {
      //generate new list
      var item = (Store)basedOnCombo.SelectedItem;
      if ( item != null )
        LoadEventsData( item );
    }

    private void LoadEventsData( Store item )
    {
      actions.Clear();

      EventInfo[] eia = item.ClassType.GetEvents();

      eventCombo.BeginUpdate();
      eventCombo.DataSource = null;

      foreach ( var ei in eia )
      {
        //get the event name

        ParameterInfo[] epia = ei.EventHandlerType.GetMethod( "Invoke" ).GetParameters();

        string eventarg = String.Empty;

        if ( epia != null )
        {
          foreach ( var pi in epia )
          {
            if ( pi.ParameterType == typeof( EventArgs ) || pi.ParameterType.IsSubclassOf( typeof( EventArgs ) ) )
            {
              eventarg = pi.ParameterType.FullName; //because we otherwise get a lot of issues building stuff when the class is not in the standard System.Windows.Forms namespace

              //System.Console.WriteLine(ei.Name + " : " + ei.EventHandlerType + " :: " + "" + eventarg);
              actions.Add( new ViewAction() { EventName = ei.Name, EventHandlerName = ei.EventHandlerType.FullName, EventArgsName = eventarg } );

              break;
            }
          }
        }

      }
      eventCombo.DataSource = actions;
      eventCombo.DisplayMember = "EventName";
      eventCombo.EndUpdate();


    }

    #region IModalSubFormContainer Members

    public bool PerformModalTask<T, R>( T data, ref R result )
    {
      if ( View.ShowDialog() == DialogResult.OK )
      {
        //get the selected item
        var selected = eventCombo.SelectedItem;

        result = (R)(object)selected;
        return ( selected != null );
      }
      else
        return false;
    }

    public bool PerformModalTask<T>( T data )
    {
      throw new NotImplementedException();
    }

    public bool PerformModalTask()
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}

