using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RatCow.Utilities
{
  /// <summary>
  /// Okay, this is making something that is pretty simple vaguely simpler. I do this to remind myself
  /// *how* to do it!!
  /// </summary>
  public sealed class CallbackUtils
  {
    public delegate void PostMessageDelegate();

    private System.Windows.Forms.Control _target = null;

    public CallbackUtils( System.Windows.Forms.Control target )
    {
      _target = target;
    }

    /// <summary>
    /// Call this method Asynchranously
    /// </summary>
    /// <param name="callback"></param>
    public void PostMessage( PostMessageDelegate callback )
    {
      _target.Invoke( callback );
    }

    /// <summary>
    /// This will attempt to invoke anything
    /// </summary>
    /// <param name="target"></param>
    /// <param name="callback"></param>
    /// <param name="args"></param>
    public void PostMessage( Delegate callback, params object[] args )
    {
      _target.Invoke( callback, args );
    }

    /// <summary>
    /// Call this method Asynchranousl
    /// </summary>
    /// <param name="callback"></param>
    public static void PostMessage( System.Windows.Forms.Control target, PostMessageDelegate callback )
    {
      target.Invoke( callback );
    }

    /// <summary>
    /// This will attempt to invoke anything
    /// </summary>
    /// <param name="target"></param>
    /// <param name="callback"></param>
    /// <param name="args"></param>
    public static void PostMessage( System.Windows.Forms.Control target, Delegate callback, params object[] args )
    {
      target.Invoke( callback, args );
    }
  }
}
