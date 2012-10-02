using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Reflection;

namespace RatCow.MvcFramework.WPF
{
  public class BaseController<T>: IDisposable
  {

    protected T target;

    /// <summary>
    /// required internally
    /// </summary>
    /// <returns></returns>
    private static Type GetTargetType()
    {
      Type t = typeof( T );
      return t;
    }

    public BaseController()
    {
      //Create new instance of T
      Type t = typeof( T );
      target = (T)Activator.CreateInstance( t );

      //attach the outlets and actions
      FixUp();
      FixUpView();
    }

    public BaseController( T aTarget )
    {
      target = aTarget;

      //attach the outlets and actions
      FixUp();
      FixUpView();
    }

    private void FixUpView()
    {
      //throw new NotImplementedException();
    }

    /// <summary>
    /// This will build all of the internl look-up structures
    /// </summary>
    protected virtual void FixUp()
    {
      Type t = typeof( T );
      Type self = this.GetType();

      //look for attributes

      BindingFlags bindingFlags = ( BindingFlags.Instance | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.FlattenHierarchy );

      //search for all Outlets
      PropertyInfo[] pia = self.GetProperties();
      foreach ( var pi in pia )
      {
        OutletAttribute[] outlets = (OutletAttribute[])( pi.GetCustomAttributes( typeof( OutletAttribute ), true ) );
        foreach ( var outlet in outlets )
        {
          //do something
          FieldInfo fi = t.GetField( outlet.Name, bindingFlags );

          if ( fi != null )
          {
            object value = fi.GetValue( target ); //this is the value pointing to the control to hook

            pi.SetValue( this, value, null );

            //search for all Actions
            MethodInfo[] mia = self.GetMethods(); // might need to revisit binding flags?
            foreach ( var mi in mia )
            {
              ActionAttribute[] actions = (ActionAttribute[])( mi.GetCustomAttributes( typeof( ActionAttribute ), true ) );
              foreach ( var action in actions )
              {
                if ( action.Name == outlet.Name )
                {
                  Type ct = fi.FieldType;
                  //do something
                  EventInfo ei = ct.GetEvent( action.Action, bindingFlags );
                  if ( ei != null )
                  {
                    Type evt = ( action.EventType == null ? ei.EventHandlerType : action.EventType );
                    Delegate temp = Delegate.CreateDelegate( evt, this, mi, false );
                    ei.AddEventHandler( value, temp );
                  }
                }
              }
            }
          }
        }
      }
    }

    public T View { get { return target; } }


    #region IDisposable Members

    public void Dispose()
    {
      throw new NotImplementedException();
    }

    #endregion
  }
}
