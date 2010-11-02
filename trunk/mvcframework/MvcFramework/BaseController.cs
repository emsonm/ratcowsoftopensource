/*
 * Copyright 2010 Rat Cow Software and Matt Emson. All rights reserved.
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
using System.Collections.Generic;
#if !CF_20
using System.Linq;
#endif
using System.Text;

using System.Reflection;
using System.Windows.Forms;

namespace RatCow.MvcFramework
{
  public class BaseController<T>
  {
    protected T target;

    /// <summary>
    /// required internally
    /// </summary>
    /// <returns></returns>
    static Type GetTargetType()
    {
      Type t = typeof(T);
      return t;
    }

    public BaseController()
    {
      //Create new instance of T
      Type t = typeof(T);
      target = (T)Activator.CreateInstance(t);

      //attach the outlets and actions
      FixUp();
      FixUpView();
    }

    public BaseController(T aTarget)
    {
      target = aTarget;

      //attach the outlets and actions
      FixUp();
      FixUpView();
    }

    /// <summary>
    /// This will build all of the internl look-up structures
    /// </summary>
    protected virtual void FixUp()
    {
      Type t = typeof(T);
      Type self = this.GetType();

      //look for attributes

      BindingFlags bindingFlags = (BindingFlags.Instance | BindingFlags.GetField | BindingFlags.GetProperty | BindingFlags.SetField | BindingFlags.SetProperty | BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.FlattenHierarchy);

      //search for all Outlets
      PropertyInfo[] pia = self.GetProperties();
      foreach (var pi in pia)
      {
        OutletAttribute[] outlets = (OutletAttribute[])(pi.GetCustomAttributes(typeof(OutletAttribute), true));
        foreach (var outlet in outlets)
        {
          //do something
          FieldInfo fi = t.GetField(outlet.Name, bindingFlags); 

          if (fi != null)
          {
            object value = fi.GetValue(target); //this is the value pointing to the control to hook

            pi.SetValue(this, value, null);

            //search for all Actions
            MethodInfo[] mia = self.GetMethods(); // might need to revisit binding flags?
            foreach (var mi in mia)
            {
              ActionAttribute[] actions = (ActionAttribute[])(mi.GetCustomAttributes(typeof(ActionAttribute), true));
              foreach (var action in actions)
              {
                if (action.Name == outlet.Name)
                {
                  Type ct = fi.FieldType;
                  //do something
                  EventInfo ei = ct.GetEvent(action.Action, bindingFlags);
                  if (ei != null)
                  {
#if !USE_COMPACTFRAMEWORK
                    Type evt = (action.EventType == null ? ei.EventHandlerType : action.EventType);
                    Delegate temp = Delegate.CreateDelegate(evt, this, mi, false);
                    ei.AddEventHandler(value, temp);
#else
#if CF_35
                    Type evt = (action.EventType == null ? ei.EventHandlerType : action.EventType);
                    Delegate temp = Delegate.CreateDelegate(evt, this, mi); //, false); <--CF doesn't like the 4th param
                    ei.AddEventHandler(value, temp);
#else
                    //I might want to 
                    EventHandler temp = new EventHandler(new EventHandlerProxy(mi, this).ProxyEventMethod);
                    ei.AddEventHandler(value, temp);
#endif
#endif

                  }
                }
              }
            }
          }
        }
      }
    }

    /// <summary>
    /// Creates event handlers for the view
    /// </summary>
    /// <param name="?"></param>
    void FixUpView()
    {
      Form view = (Form)((object)target);

      view.Load += new EventHandler(View_Load);
    }

    //handeler to override
    protected virtual void ViewLoad()
    {
    }
    
    /// <summary>
    /// Hardcoded onload
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void View_Load(object sender, EventArgs e)
    {
      ViewLoad();
    }



    #region CompactFramework 2.0 Delegate suport


#if USE_COMPACTFRAMEWORK && CF_20

    //This is required for CF 2.0 support as Delegate.CreateDelegate does not exist
    internal class EventHandlerProxy
    {
      MethodInfo fmi = null;
      object ftarget = null;

      public EventHandlerProxy(MethodInfo mi, object target)
      {
        fmi = mi;
        ftarget = target;
      }

      public void ProxyEventMethod(object sender, EventArgs e)
      {
        fmi.Invoke(ftarget, new object[] { sender, e });
      }
    }

#endif

    #endregion


    public T View { get { return target; } }


    //the view can own subviews - we generally have two types: 
    // 1) Modal dialogs
    // 2) Floating forms
    //At the moment, only Modal dialogs are being catered for... 
    protected Dictionary<string, IModalSubFormContainer> fModalSubControllers = new Dictionary<string, IModalSubFormContainer>();
    public void AddModalSubController(string name, IModalSubFormContainer controller)
    {
      fModalSubControllers.Add(name, controller);
    }

    public bool ExecuteModalController(string name)
    {
      bool result = false;

      IModalSubFormContainer controller = null;

      if (fModalSubControllers.TryGetValue(name, out controller))
      {
        result = controller.PerformModalTask();
      }

      return result;
    }


  }
}
