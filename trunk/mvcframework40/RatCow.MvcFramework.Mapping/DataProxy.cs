/*
 * Copyright 2007 - 2012 Rat Cow Software and Matt Emson. All rights reserved.
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
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace RatCow.MvcFramework.Mapping
{
  /// <summary>
  /// This class sits as a proxy between two objects, the MappingObject and a source of the data
  /// </summary>
  public partial class DataProxy
  {
    public DataProxy()
    {
      MappingList = new List<IMappingObject>();
    }

    //this retains all of out dynamicly mapped connections
    public List<IMappingObject> MappingList { get; private set; }

    //maps the dynamic data links
    /// <summary>
    /// Believe it or not, this is all new code! Yeah, I know..
    ///
    /// The old version used the MappedObjects as part of the DataObject, but that doesn't work/scale well if we
    /// start throwing Linq and code first database stuff with EF4.3.
    /// </summary>
    public void MapControlToData(string usage, System.Windows.Forms.Control control, object data)
    {
      //set up what we define as "default"
      bool useDefaultMapping = (usage == String.Empty || usage.ToLower() == "default");

      //we iterrate through all of the propertues in data looking for the [Mapping] attribute
      Type dataType = data.GetType();
      PropertyInfo[] pia = dataType.GetProperties(); //we only want the public props
      foreach (var pi in pia)
      {
        //we now look for the MappedValueAttribute for the data we were sent
        MappedValueAttribute[] maa = (MappedValueAttribute[])(pi.GetCustomAttributes(typeof(MappedValueAttribute), true));
        if (maa.Length > 0) //we have something to do?
        {
          foreach (var ma in maa)
          {
            var isDefaultItem = useDefaultMapping && ma.Usage == String.Empty;

            if ((usage == ma.Usage) || (useDefaultMapping && isDefaultItem))
            {
              //create the control mapping
              IMappingObject mo;

              //we now need to find the mapped control
              Control controlToHook = null;
              if (ControlHelper.FindControl(control, ma.DestinationControlName, ref controlToHook))
              {
                var isListcontrol = (controlToHook is ListControl);

                mo = GenericsHelper.CreateGenericInstance<IMappingObject>(typeof(MappingObject<>), pi.PropertyType);

                mo.InitWithObject(data, pi.Name);

                if (isListcontrol && ma.ListMappedByIndex)
                  mo.Subscribe(controlToHook, ListControlMapping.Index);
                else if (isListcontrol && ma.ListMappedByText)
                  mo.Subscribe(controlToHook, ListControlMapping.Text);
                else
                  mo.Subscribe(controlToHook); //subscribe to updates

                mo.LinkedControl = controlToHook;
                mo.Pull(true); //pull the current data through to the mapped control
                mo.ValueModificationQuery += new DataModificationDelegate(mo_ValueModified);
                mo.ValueWasModified += new EventHandler(mo_ValueWasModified);
                mo.ValidationError += new ValidationErrorDelegate(mo_ValidationError);
                mo.BeforeValueModified += new BeforeDataModificationDelegate(mo_BeforeValueModified);
                MappingList.Add(mo); //add the now working mapping
              }
              else throw new NotImplementedException("The control was not found and cannot be hooked");
            }
          }
        }
      }
    }

    #region This code defines 4 virtual methods that pass the events fired by the various MappingObjects

    private void mo_BeforeValueModified(object sender, BeforeDataModification e)
    {
      BeforeValueModified(sender, e);
    }

    protected virtual void BeforeValueModified(object sender, BeforeDataModification e)
    {
    }

    private void mo_ValidationError(object sender, string message)
    {
      ValidationError(sender, message);
    }

    protected virtual void ValidationError(object sender, string message)
    {
    }

    private void mo_ValueModified(object sender, DataModificationArgs e)
    {
      ValueModifiedQuery(sender, e);
    }

    protected virtual void ValueModifiedQuery(object sender, DataModificationArgs e)
    {
    }

    private void mo_ValueWasModified(object sender, EventArgs e)
    {
      ValueWasModified(sender, e);
    }

    public virtual void ValueWasModified(object sender, EventArgs e)
    {
    }

    #endregion This code defines 4 virtual methods that pass the events fired by the various MappingObjects
  }
}