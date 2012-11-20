using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Ingenium.WF2XAML.Commands
{
  public class DelegateCommand : ICommand
  {
    private readonly Action _executeMethod;

    private readonly Func<bool> _canExecuteMethod;

    private bool _isAutomaticRequeryDisabled;

    private List<WeakReference> _canExecuteChangedHandlers;

    public bool IsAutomaticRequeryDisabled
    {
      get
      {
        return this._isAutomaticRequeryDisabled;
      }
      set
      {
        if ( this._isAutomaticRequeryDisabled != value )
        {
          if ( !value )
          {
            CommandManagerHelper.AddHandlersToRequerySuggested( this._canExecuteChangedHandlers );
          }
          else
          {
            CommandManagerHelper.RemoveHandlersFromRequerySuggested( this._canExecuteChangedHandlers );
          }
          this._isAutomaticRequeryDisabled = value;
        }
      }
    }

    public DelegateCommand( Action executeMethod )
      : this( executeMethod, null, false )
    {
    }

    public DelegateCommand( Action executeMethod, Func<bool> canExecuteMethod )
      : this( executeMethod, canExecuteMethod, false )
    {
    }

    public DelegateCommand( Action executeMethod, Func<bool> canExecuteMethod, bool isAutomaticRequeryDisabled )
    {
      if ( executeMethod != null )
      {
        this._executeMethod = executeMethod;
        this._canExecuteMethod = canExecuteMethod;
        this._isAutomaticRequeryDisabled = isAutomaticRequeryDisabled;
        return;
      }
      else
      {
        throw new ArgumentNullException( "executeMethod" );
      }
    }

    public bool CanExecute()
    {
      if ( this._canExecuteMethod == null )
      {
        return true;
      }
      else
      {
        return this._canExecuteMethod();
      }
    }

    public void Execute()
    {
      if ( this._executeMethod != null )
      {
        this._executeMethod();
      }
    }

    protected virtual void OnCanExecuteChanged()
    {
      CommandManagerHelper.CallWeakReferenceHandlers( this._canExecuteChangedHandlers );
    }

    public void RaiseCanExecuteChanged()
    {
      this.OnCanExecuteChanged();
    }

    bool System.Windows.Input.ICommand.CanExecute( object parameter )
    {
      return this.CanExecute();
    }

    void System.Windows.Input.ICommand.Execute( object parameter )
    {
      this.Execute();
    }

    public event EventHandler CanExecuteChanged
    {
      add
      {
        if ( !this._isAutomaticRequeryDisabled )
        {
          CommandManager.RequerySuggested += value;
        }
        CommandManagerHelper.AddWeakReferenceHandler( ref this._canExecuteChangedHandlers, value, 2 );
      }
      remove
      {
        if ( !this._isAutomaticRequeryDisabled )
        {
          CommandManager.RequerySuggested -= value;
        }
        CommandManagerHelper.RemoveWeakReferenceHandler( this._canExecuteChangedHandlers, value );
      }
    }
  }
}