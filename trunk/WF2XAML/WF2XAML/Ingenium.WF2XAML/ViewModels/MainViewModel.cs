using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Markup;
using Ingenium.WF2XAML.Commands;
using Ingenium.WF2XAML.Models;
using Ingenium.WF2XAML.Parser;
using Ingenium.WF2XAML.Views;
using SWF = System.Windows.Forms;

namespace Ingenium.WF2XAML.ViewModels
{
  public class MainViewModel : ViewModelBase
  {
    private WinFormConverter _formConverter;

    private DelegateCommand exitCommand;

    private DelegateCommand convertSourceCommand;

    private DelegateCommand copyToClipboardCommand;

    private Type _selectedForm;

    private ProjectType _selectedProjectType;

    private List<ProjectType> _projectTypes;

    private ObservableCollection<Type> _assemblyForms;

    private bool _showPreview;

    public ObservableCollection<Type> AssemblyForms
    {
      get
      {
        return this._assemblyForms;
      }
    }

    public ICommand ConvertSourceCommand
    {
      get
      {
        if ( this.convertSourceCommand == null )
        {
          this.convertSourceCommand = new DelegateCommand( new Action( this.ConvertSource ) );
        }
        return this.convertSourceCommand;
      }
    }

    public ICommand CopyToClipboardCommand
    {
      get
      {
        if ( this.copyToClipboardCommand == null )
        {
          this.copyToClipboardCommand = new DelegateCommand( new Action( this.CopyToClipboard ) );
        }
        return this.copyToClipboardCommand;
      }
    }

    public string Destination
    {
      get
      {
        return this._formConverter.DestFilePath;
      }
      set
      {
        this._formConverter.DestFilePath = value;
        base.OnPropertyChanged( "Destination" );
      }
    }

    public ICommand ExitCommand
    {
      get
      {
        if ( this.exitCommand == null )
        {
          this.exitCommand = new DelegateCommand( new Action( this.Exit ) );
        }
        return this.exitCommand;
      }
    }

    public ObservableCollection<ParserError> ParserErrors
    {
      get
      {
        return new ObservableCollection<ParserError>( this._formConverter.ParserErrors );
      }
    }

    public List<ProjectType> ProjectTypes
    {
      get
      {
        return this._projectTypes;
      }
      set
      {
        this._projectTypes = value;
      }
    }

    public string Result
    {
      get
      {
        if ( this._formConverter == null )
        {
          return string.Empty;
        }
        else
        {
          return this._formConverter.Result;
        }
      }
    }

    public Type SelectedForm
    {
      get
      {
        return this._selectedForm;
      }
      set
      {
        this._selectedForm = value;
      }
    }

    public ProjectType SelectedProjectType
    {
      get
      {
        return this._selectedProjectType;
      }
      set
      {
        this._selectedProjectType = value;
        base.OnPropertyChanged( "SelectedProjectType" );
      }
    }

    public bool ShowPreview
    {
      get
      {
        return this._showPreview;
      }
      set
      {
        this._showPreview = value;
        base.OnPropertyChanged( "ShowPreview" );
      }
    }

    public string Source
    {
      get
      {
        return this._formConverter.SourceFilePath;
      }
      set
      {
        Assembly assembly = Assembly.LoadFrom( value );
        Type[] types = assembly.GetTypes();
        this._assemblyForms = new ObservableCollection<Type>();
        Type[] typeArray = types;
        foreach ( Type list in (IEnumerable<Type>)typeArray.Where<Type>( ( Type t ) => t.IsSubclassOf( typeof( SWF.Form ) ) ).ToList<Type>() )
        {
          this._assemblyForms.Add( list );
        }
        base.OnPropertyChanged( "Source" );
        base.OnPropertyChanged( "AssemblyForms" );
      }
    }

    public string Title
    {
      get
      {
        return "in:genium Software Engineering - WinForm(2)XAML Converter - v.0.6.2.0 BETA";
      }
    }

    public MainViewModel()
    {
      this._projectTypes = new List<ProjectType>();
      this._projectTypes.Add( new ProjectType( 0, "WPF Window" ) );
      this._projectTypes.Add( new ProjectType( 1, "WPF Page" ) );
      this._projectTypes.Add( new ProjectType( 2, "WPF User Control" ) );
      this._projectTypes.Add( new ProjectType( 3, "Silverlight User Control" ) );
      this._projectTypes.Add( new ProjectType( 4, "Windows Phone 7" ) );
      this.SelectedProjectType = this._projectTypes[ 0 ];
    }

    private void ConvertSource()
    {
      if ( this._selectedForm != null )
      {
        this._formConverter = new WinFormConverter();
        this._formConverter.Properties.Add( "Namespace", "" );
        this._formConverter.Properties.Add( "ClassName", this._selectedForm.Name );
        this._formConverter.RootContainerType = (WinFormConverter.RootContainerTypes)this._selectedProjectType.Code;
        try
        {
          SWF.Form form = (SWF.Form)Activator.CreateInstance( this._selectedForm );
          if ( this._formConverter.Convert( form ) && this._showPreview )
          {
            if ( this._selectedProjectType.Code == 4 )
            {
              MessageBox.Show( "Can't show preview for Windows Phone destination.", "Information", MessageBoxButton.OK, MessageBoxImage.Asterisk );
            }
            else
            {
              try
              {
                using ( Stream memoryStream = new MemoryStream( Encoding.Default.GetBytes( this._formConverter.Result ) ) )
                {
                  object obj = XamlReader.Load( memoryStream );
                  if ( obj.GetType() != typeof( Window ) )
                  {
                    if ( obj.GetType() == typeof( UserControl ) )
                    {
                      HostWindow hostWindow = new HostWindow();
                      hostWindow.InjectControl( (UserControl)obj );
                      hostWindow.Show();
                    }
                  }
                  else
                  {
                    Window window = (Window)obj;
                    window.Show();
                  }
                }
              }
              catch ( Exception exception1 )
              {
                Exception exception = exception1;
                this._formConverter.ParserErrors.Add( new ParserError( 0, string.Format( "Error while rendering XAML: {0}", exception.Message ) ) );
              }
            }
          }
        }
        catch ( Exception exception3 )
        {
          Exception exception2 = exception3;
          this._formConverter.ParserErrors.Add( new ParserError( 0, string.Format( "Error: {0}", exception2.Message ) ) );
        }
        base.OnPropertyChanged( "ParserErrors" );
        base.OnPropertyChanged( "Result" );
      }
    }

    private void CopyToClipboard()
    {
      Clipboard.SetText( this.Result );
    }

    private void Exit()
    {
      Application.Current.Shutdown();
    }

    private class UnknownControlsCollection : ObservableCollection<string>
    {
      private List<string> _innerList;

      public UnknownControlsCollection()
      {
      }
    }
  }
}