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
  public class DataEnabledObject
  {
    public DataEnabledObject()
    {
      MappingList = new List<IMappingObject>();
    }

    //we have two maps - this is legacy, not sure we need them...
    protected Dictionary<string, string> dataToControlMap = new Dictionary<string, string>();
    protected Dictionary<string, string> controlToDataMap = new Dictionary<string, string>();

    //this retains all of out dynamicly mapped connections
    public List<IMappingObject> MappingList { get; private set; }

    //maps the dynamic data links
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
              dataToControlMap.Add(ma.DestinationControlName, pi.Name);
              controlToDataMap.Add(pi.Name, ma.DestinationControlName);

              //create the control mapping
              IMappingObject mo = GenericsHelper.CreateGenericInstance<IMappingObject>(typeof(MappingObject<>), pi.PropertyType);
              mo.InitWithObject(data, pi.Name);

              //we now need to find the mapped control
              Control controlToHook = null;
              if (ControlHelper.FindControl(control, ma.DestinationControlName, ref controlToHook))
              {
                mo.Subscribe(controlToHook); //subscribe to updates
                mo.Pull(true);
                MappingList.Add(mo); //add the now working mapping
              }
              else throw new NotImplementedException("The control was not found and cannot be hooked");
            }
          }
        }
      }
    }
  }
}