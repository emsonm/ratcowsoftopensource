using System;
using System.Collections.Generic;
using System.Windows.Input;

namespace Ingenium.WF2XAML.Commands
{
  internal class CommandManagerHelper
  {
    // Methods
    internal static void AddHandlersToRequerySuggested( List<WeakReference> handlers )
    {
      if ( handlers != null )
      {
        foreach ( WeakReference reference in handlers )
        {
          EventHandler target = reference.Target as EventHandler;
          if ( target != null )
          {
            CommandManager.RequerySuggested += target;
          }
        }
      }
    }

    internal static void AddWeakReferenceHandler( ref List<WeakReference> handlers, EventHandler handler )
    {
      AddWeakReferenceHandler( ref handlers, handler, -1 );
    }

    internal static void AddWeakReferenceHandler( ref List<WeakReference> handlers, EventHandler handler, int defaultListSize )
    {
      if ( handlers == null )
      {
        handlers = ( defaultListSize > 0 ) ? new List<WeakReference>( defaultListSize ) : new List<WeakReference>();
      }
      handlers.Add( new WeakReference( handler ) );
    }

    internal static void CallWeakReferenceHandlers( List<WeakReference> handlers )
    {
      if ( handlers != null )
      {
        EventHandler[] handlerArray = new EventHandler[ handlers.Count ];
        int index = 0;
        for ( int i = handlers.Count - 1 ; i >= 0 ; i-- )
        {
          WeakReference reference = handlers[ i ];
          EventHandler target = reference.Target as EventHandler;
          if ( target == null )
          {
            handlers.RemoveAt( i );
          }
          else
          {
            handlerArray[ index ] = target;
            index++;
          }
        }
        for ( int j = 0 ; j < index ; j++ )
        {
          EventHandler handler2 = handlerArray[ j ];
          handler2( null, EventArgs.Empty );
        }
      }
    }

    internal static void RemoveHandlersFromRequerySuggested( List<WeakReference> handlers )
    {
      if ( handlers != null )
      {
        foreach ( WeakReference reference in handlers )
        {
          EventHandler target = reference.Target as EventHandler;
          if ( target != null )
          {
            CommandManager.RequerySuggested -= target;
          }
        }
      }
    }

    internal static void RemoveWeakReferenceHandler( List<WeakReference> handlers, EventHandler handler )
    {
      if ( handlers != null )
      {
        for ( int i = handlers.Count - 1 ; i >= 0 ; i-- )
        {
          WeakReference reference = handlers[ i ];
          EventHandler target = reference.Target as EventHandler;
          if ( ( target == null ) || ( target == handler ) )
          {
            handlers.RemoveAt( i );
          }
        }
      }
    }
  }
}