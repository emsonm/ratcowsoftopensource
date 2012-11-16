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
using System.IO;

//3rd Party
using RatCow.MvcFramework;
using RatCow.MvcFramework.Tools;
using RatCow.PluginFramework;

using BrendanGrant.Helpers.FileAssociation;

namespace RatCow.MvcFramework.MvcMapTool
{
  /// <summary>
  /// 
  /// </summary>
  internal partial class MainFormController : BaseController<MainForm>
  {
    ListViewHelper<ViewAction> globalEvents = null;
    ListViewHelper<ViewControlAction> controls = null;
    ListViewHelper<ViewAction> controlEvents = null;
    ListViewHelper<ViewAssembly> externalAssemblyHelper = null;

    const string GENERAL_DLG = "GeneralDialog";
    const string CONTROL_DLG = "ControlDialog";
    const string CONTROLEVENTS_DLG = "ControlEventsDialog";

    ViewActionMap data = null;
    bool modified = false;
    string fileLoaded = String.Empty;

    /// <summary>
    /// 
    /// </summary>
    protected override void ViewLoad()
    {
      base.ViewLoad();

      InitFileAssociations();

      AddModalSubController( GENERAL_DLG, new AddGeneralItemFormController() );
      AddModalSubController( CONTROL_DLG, new AddControlFormController() );
      AddModalSubController( CONTROLEVENTS_DLG, new AddControlEventsFormController() );

      //load the file
      string[] items = Environment.GetCommandLineArgs();
      if ( items.Length > 1 )
      {
        //Assume items[1] is passed path
        LoadData( items[ 1 ] );
      }
      else LoadData( String.Empty );

      globalEvents = GetglobalEventsListViewHelper<ViewAction>();
      globalEvents.SetData( data.GlobalMap );

      controls = GetcontrolsListViewHelper<ViewControlAction>();
      controls.SetData( data.ControlActionMap );

      controlEvents = GetcontrolDetailListViewHelper<ViewAction>();

      externalAssemblyHelper = GetexternalAssembliesHelper<ViewAssembly>();
      externalAssemblyHelper.SetData( data.AssemblyDependencies );

      LoadAssemblies();
    }



    /// <summary>
    /// 
    /// </summary>
    void InitFileAssociations()
    {
      FileAssociationInfo fai = new FileAssociationInfo( ".mvcmap" );
      if ( !fai.Exists )
      {
        fai.Create( "RatCow.MvcFramework.MVCMAP" );
        fai.ContentType = "application/mvcmap-xml";
        //Programs automatically displayed in open with list
        fai.OpenWithList = new string[] { "mvcmaptool.exe", "notepad.exe", "wordpad.exe", "sublimeedit2.exe" };
      }

      ProgramAssociationInfo pai = new ProgramAssociationInfo( fai.ProgID );
      if ( !pai.Exists )
      {
        pai.Create
        (
          //Description of program/file type
          "RatCow Soft MvcFramework MVCMAP editor",

          new ProgramVerb(
            "Open",//Verb name
            String.Format( "{0} %1", Application.ExecutablePath ) //Path and arguments to use
          )
         );
      }
    }

    /// <summary>
    /// 
    /// </summary>
    private void LoadData( string path )
    {
      if ( File.Exists( path ) )
      {
        data = ViewActionMap.Load( path );
        fileLoaded = path;
      }
      else if ( File.Exists( "default.mvcmap" ) )
      {
        data = ViewActionMap.Load( "default.mvcmap" );
        fileLoaded = "default.mvcmap";
      }
      else
        data = new ViewActionMap( true ); //fallback
    }

    #region ListView stuff

    partial void externalAssembliesGetItem( ref ListViewItem item, RetrieveVirtualItemEventArgs e )
    {
      if ( item == null )
        item = new ListViewItem();

      item.Text = data.AssemblyDependencies[ e.ItemIndex ].AssemblyName;
    }

    /// <summary>
    /// 
    /// </summary>
    partial void globalEventsListViewGetItem( ref ListViewItem item, RetrieveVirtualItemEventArgs e )
    {
      if ( item == null )
        item = new ListViewItem();

      item.Text = data.GlobalMap[ e.ItemIndex ].EventName;
      item.SubItems.Add( data.GlobalMap[ e.ItemIndex ].EventHandlerName );
      item.SubItems.Add( data.GlobalMap[ e.ItemIndex ].EventArgsName );
    }

    /// <summary>
    /// 
    /// </summary>
    partial void controlsListViewGetItem( ref ListViewItem item, RetrieveVirtualItemEventArgs e )
    {
      if ( item == null )
        item = new ListViewItem();

      item.Text = data.ControlActionMap[ e.ItemIndex ].ControlType;
    }

    /// <summary>
    /// 
    /// </summary>
    partial void controlDetailListViewGetItem( ref ListViewItem item, RetrieveVirtualItemEventArgs e )
    {
      if ( item == null )
        item = new ListViewItem();

      var target = controls.GetSelectedItemOrDefault();
      if ( target != null )
      {
        item.Text = target.ControlActions[ e.ItemIndex ].EventName;
        item.SubItems.Add( target.ControlActions[ e.ItemIndex ].EventHandlerName );
        item.SubItems.Add( target.ControlActions[ e.ItemIndex ].EventArgsName );
      }
    }

    /// <summary>
    /// 
    /// </summary>
    partial void controlsListViewSelectedIndexChanged( EventArgs e )
    {
      var target = controls.GetSelectedItemOrDefault();

      if ( target != null )
        controlEvents.SetData( target.ControlActions );
      else
        controlEvents.SetData( null );
    }

    #endregion

    /// <summary>
    /// 
    /// </summary>
    partial void addGeneralButtonClick( EventArgs e )
    {
      ViewAction newAction = null;
      if ( ExecuteModalControllerWithData<int, ViewAction>( GENERAL_DLG, 0, ref newAction ) )
      {
        if ( newAction != null )
        {
          //we should never have 2 global events with the same name.... even if that is valid.
          if ( data.GlobalMap.Where( x => x.EventName == newAction.EventName ).Count() <= 0 )
          {
            data.GlobalMap.Add( newAction );
            globalEvents.SetData( data.GlobalMap );

            modified = true;
          }
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    partial void removeGeneralButtonClick( EventArgs e )
    {
      var selectedGlobal = globalEvents.GetSelectedItemOrDefault();

      if ( selectedGlobal != null )
      {
        data.GlobalMap.Remove( selectedGlobal );
        globalEvents.SetData( data.GlobalMap );
      }
    }

    /// <summary>
    /// 
    /// </summary>
    partial void addControlClick( EventArgs e )
    {
      ViewControlAction newAction = null;
      if ( ExecuteModalControllerWithData<List<Assembly>, ViewControlAction>( CONTROL_DLG, Externals, ref newAction ) )
      {
        if ( newAction != null )
        {
          //does this already exist in the list? don't duplicate
          if ( data.ControlActionMap.Where( x => x.ControlType == newAction.ControlType ).Count() <= 0 )
          {
            data.ControlActionMap.Add( newAction );
            controls.SetData( data.ControlActionMap );

            modified = true;
          }
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    partial void removeControlClick( EventArgs e )
    {
      var selectedControl = controls.GetSelectedItemOrDefault();

      if ( selectedControl != null )
      {
        data.ControlActionMap.Remove( selectedControl );
        controls.SetData( data.ControlActionMap );
      }
    }

    /// <summary>
    /// 
    /// </summary>
    partial void addEventClick( EventArgs e )
    {
      ViewAction newAction = null;

      var selectedControl = controls.GetSelectedItemOrDefault();
      if ( selectedControl != null )
      {
        if ( ExecuteModalControllerWithData<object[], ViewAction>( CONTROLEVENTS_DLG, new object[] { selectedControl.ControlType, Externals }, ref newAction ) )
        {
          if ( newAction != null )
          {
            //does this already exist in the list? don't duplicate
            if ( selectedControl.ControlActions.Where( x => x.EventName == newAction.EventName ).Count() <= 0 )
            {
              selectedControl.ControlActions.Add( newAction );
              controlEvents.SetData( selectedControl.ControlActions );

              modified = true;
            }
          }
        }
      }
    }

    /// <summary>
    /// 
    /// </summary>
    partial void removeEventClick( EventArgs e )
    {
      var selectedControl = controls.GetSelectedItemOrDefault();
      var selectedControlEvent = controlEvents.GetSelectedItemOrDefault();

      if ( selectedControl != null && selectedControlEvent != null )
      {
        selectedControl.ControlActions.Remove( selectedControlEvent );
        controlEvents.SetData( selectedControl.ControlActions );
      }
    }

    /// <summary>
    /// 
    /// </summary>
    partial void saveButtonClick( EventArgs e )
    {
      if ( fileLoaded == String.Empty )
      {
        //first we get a filename
        using ( var fileDialog = new SaveFileDialog() )
        {
          fileDialog.Filter = "MVCMAP Files|*.mvcmap";
          fileDialog.InitialDirectory = Environment.CurrentDirectory;
          fileDialog.OverwritePrompt = true;
          fileDialog.Title = "Save file as...";
          if ( fileDialog.ShowDialog() == DialogResult.OK )
          {
            fileLoaded = fileDialog.FileName;
          }
          else
            UIBridge.Alert( "File save aborted" );
        }
      }

      //we just overwrite... should this be an option?

      ViewActionMap.Save( data, fileLoaded, true );

      modified = false;
    }

    /// <summary>
    /// 
    /// </summary>
    protected override void ViewClosing( FormClosingEventArgs e )
    {
      if ( modified )
        e.Cancel = UIBridge.BooleanDecision( "Save changes?" );
    }

    /// <summary>
    /// 
    /// </summary>
    partial void closeButtonClick( EventArgs e )
    {
      View.Close();
    }

    List<Assembly> Externals = new List<Assembly>();

    partial void addAssemblyClick( EventArgs e )
    {
      using ( var dlg = new OpenFileDialog() )
      {
        dlg.Filter = "*.dll|*.dll";
        if ( dlg.ShowDialog() == DialogResult.OK )
        {
          //get the assembly
          try
          {
            LoadAssemblyData( dlg.FileName, true );

            modified = true;
          }
          catch ( Exception ex )
          {
            System.Diagnostics.Debug.WriteLine( ex.Message );
          }
        }
      }
    }

    partial void removeAssemblyClick( EventArgs e )
    {
      var selected = externalAssemblyHelper.GetSelectedItemOrDefault();

      if ( selected != null )
      {
        data.AssemblyDependencies.Remove( selected );
        externalAssemblyHelper.SetData( data.AssemblyDependencies );
        LoadAssemblies();
      }
    }

    private void LoadAssemblyData( string fileName, bool add )
    {
      var assembly = AssemblyLoader.LoadAssemblyWithDependencies( fileName );
      if ( assembly != null )
      {
        Externals.Add( assembly );

        if ( add )
        {
          data.AssemblyDependencies.Add(
            new ViewAssembly()
            {
              AssemblyName = Path.GetFileName( fileName ),
              AssemblyFullName = assembly.FullName,
              HintPath = Path.GetDirectoryName( assembly.Location )
            }
          );
        }

        externalAssemblyHelper.SetData( data.AssemblyDependencies );
      }
    }

    private void LoadAssemblies()
    {
      Externals.Clear();

      foreach ( var assembly in data.AssemblyDependencies )
      {
        LoadAssemblyData( Path.Combine( assembly.HintPath, assembly.AssemblyName ), true );
      }
    }
  }
}

