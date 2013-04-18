using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Windows.Forms;

namespace RatCow.Utilities
{
  /// <summary>
  /// See : http://www.waytocoding.com/2011/04/how-to-remove-all-event-handlers-from.html
  /// </summary>
  public static class EventExtension
  {
    public static void RemoveEvents<T>( this Control aTarget, string aEvent )
    {
      FieldInfo fi = typeof( Control ).GetField( aEvent, BindingFlags.Static | BindingFlags.NonPublic );
      if ( fi != null )
      {
        object obj = fi.GetValue( aTarget.CastTo<T>() );
        PropertyInfo pi = aTarget.CastTo<T>().GetType().GetProperty( "Events",
            BindingFlags.NonPublic | BindingFlags.Instance );
        EventHandlerList list = (EventHandlerList)pi.GetValue( aTarget.CastTo<T>(), null );
        list.RemoveHandler( obj, list[ obj ] );
      }
    }
  }
}