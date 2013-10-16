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
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace RatCow.MvcFramework.Mapping
{
  /// <summary>
  /// The MappingObject is creater dynamically by the mapping proxy, one per mapping.
  ///
  /// This is old code. It used to support more complex mappings, but I have removed
  /// most of that code, as it was crufty. I think a newer "plug in" approach will be
  /// used instead... maybe using class helpers or something like that?
  ///
  /// Oh yeah - none of this is thread safe, so seriously, just don't try it in a
  /// multireader/singlewriter scenario!!
  /// </summary>
  public class MappingObject<T> : IMappingObject, IMappingObject<T>
  {
    /// <summary>
    /// This would be the standard point of entry.
    /// </summary>
    public MappingObject()
    {
      LinkedControl = null;
      OriginalValue = default(T);
      Modified = false;
      ValueModificationQuery = null;
      ValidationError = null;
      BeforeValueModified = null;

      isNumeric = ConversionHelper.IsNumericType(typeof(T));
    }

    /// <summary>
    /// This does the same as calling Init(...)
    /// </summary>
    public MappingObject(T aValue)
      : this()
    {
      OriginalValue = aValue;
      CurrentValue = OriginalValue;
      lastUpdated = DateTime.Now;
    }

    /// <summary>
    /// Causes the initial value to be something specific... if we call this, we BLAT the
    /// value we Pull()'d
    /// </summary>
    public void Init(T aValue)
    {
      OriginalValue = aValue;
      CurrentValue = OriginalValue;
      lastUpdated = DateTime.Now;
    }

    /// <summary>
    /// We use this to initialise the value. we cache the propertyinfo to make updates simpler. we
    /// cache the data to make updates, um, possible!
    /// </summary>
    public void InitWithObject(object data, string propertyName)
    {
      //in the original code, this was much more complicated, as we had to may some random database value to the storage. Now
      //we know they match, as we enforce that contract in the creation, so we get this dead easy version.

      dataInstance = data; //the data we are referring to
      Type dataInstanceType = dataInstance.GetType(); //the data's type, so we can get the PropertyInfo
      dataInstanceProperty = dataInstanceType.GetProperty(propertyName); //The propertyInfo we will use to pull/push updates

      if (dataInstanceProperty == null) throw new NotSupportedException("The property mapping was now found"); //exit, we have bad info passed

      //set up the storage:
      OriginalValue = (T)dataInstanceProperty.GetValue(dataInstance, null); //this is now a direct load, like for like.
      lastUpdated = DateTime.Now;

      //NB. we now wrap the propertInfo directly, so CurrentValue will read and write to the underlying data.
    }

    ////////////////////////////////////////////////

    //user can do something with the value and block modification if they like,
    //but as the original code surfaced these, and this code doesn't, the usefulness
    //is pretty debateable..
    public event DataModificationDelegate ValueModificationQuery = null;

    //this happens *after* modification
    public event System.EventHandler ValueWasModified = null;

    //added this to get around the fact that we won't ever really use inheritence here
    public event ValidationErrorDelegate ValidationError = null;

    //I couldn't remember why this was here, but it seems that the subscription uses it to relay the before modification event!
    public event BeforeDataModificationDelegate BeforeValueModified = null;

    ////////////////////////////////////////////////

    #region Fields

    //the linked control
    public Control LinkedControl { get; set; } //init to null

    //the value we read at startup, or value we last saved
    public T OriginalValue { get; protected set; } //init to default(T)

    //mapped type
    public Type MappingType { get { return typeof( T ); } }

    protected DateTime lastUpdated;

    //set to true when value is modified
    public bool Modified { get; internal set; } //init to false

    //this, IIRC, simplifies some of the discovery code.
    internal bool isNumeric = false;

    //we cache these so we don't need to pull this data on each iteration of the data manipulation
    internal object dataInstance = null;
    internal PropertyInfo dataInstanceProperty = null;

    /// <summary>
    /// Not to be confused with IsMultiValueItem - this represents a value where more than
    /// one item can be selected in a single field...
    /// </summary>
    protected bool isMultiList = false;

    /// <summary>
    /// This represents an item that points to another dataset.
    /// If this is set, we only store the fact - other code will need to load the data
    /// if we are multivalues, the value we hold is the ID of ourself, this is required to
    /// do the lookup.
    /// </summary>
    public bool IsMultiValueItem { get; set; }

    /// <summary>
    /// Subscriptions go here...
    /// </summary>
    protected internal MappingUpdateSubscriber<T> subscription = null;

    #endregion Fields

    ////////////////////////////////////////////////

    #region Properties

    //helper
    internal void SetDataInstancePropertyObjectValue(object newValue)
    {
      dataInstanceProperty.SetValue(dataInstance, newValue, null);
    }

    //helper
    internal object GetDataInstancePropertyObjectValue()
    {
      return dataInstanceProperty.GetValue(dataInstance, null);
    }

    //helper
    internal T GetDataInstancePropertyValue()
    {
      return (T)GetDataInstancePropertyObjectValue();
    }

    public T CurrentValue
    {
      get { return GetDataInstancePropertyValue(); }
      set { SetDataInstancePropertyObjectValue((object)value); }
    }

    public object CurrentObject
    {
      get { return (object)CurrentValue; }
      set { SetCurrentValueFromObject(value); }
    }

    public object OriginalObject
    {
      get { return (object)OriginalValue; }
    }

    #endregion Properties

    ////////////////////////////////////////////////

    /// <summary>
    /// This is called to check whether the user has assigned an event handler to
    /// catch data modification.
    /// </summary>
    /// <returns></returns>
    private bool CanModifyData()
    {
      if (__IN_UPDATE__)
        return false;

      bool result = true;
      if (ValueModificationQuery != null)
      {
        DataModificationArgs e = new DataModificationArgs(__BINDING__);
        ValueModificationQuery(this, e);
        result = e.AllowChange;
      }

      return result;
    }

    /// <summary>
    /// This really only helps with UI updates
    /// </summary>
    private void DataModificationNotification()
    {
      if (ValueWasModified != null)
        ValueWasModified(this, new EventArgs());
    }

    //Well, I'd forgotten this bit - the subsription uses this to relay the event.. neat?!
    internal BeforeDataModificationDelegate GetBeforeValueModified()
    {
      return BeforeValueModified;
    }

    /// <summary>
    /// allows more control over "setting" the value if needed.
    /// </summary>
    /// <param name="aValue"></param>
    protected virtual void SetCurrentValue(T aValue)
    {
      if (CanModifyData())
      {
        CurrentValue = aValue;
        lastUpdated = DateTime.Now;
        if (!__BINDING__)
        {
          Modified = true;
          DataModificationNotification();
        }
      }
    }

    /// <summary>
    /// this "should" work in a lot of simple cases
    /// </summary>
    /// <param name="newValue"></param>
    public virtual void SetCurrentValueFromObject(object newValue)
    {
      if (__IN_UPDATE__)
        return;  //aaaagh!!!

      try
      {
        if (isNumeric && (newValue == null || newValue.ToString() == String.Empty))
          newValue = 0; //hmmm will this break more than it fixes?

        var underlyingType = Nullable.GetUnderlyingType( dataInstanceProperty.PropertyType )
                 ?? dataInstanceProperty.PropertyType;

        T newValueT = ( newValue == null ) ? (T)(object)null : (T)Convert.ChangeType( newValue, underlyingType ); //bug, if currentvalue was null, failed here

        SetCurrentValue(newValueT);
      }
      catch (Exception ex) { ValidationFailed(ex.Message); } //we could not set the value because it was not valid in the context of the undelying generic type
    }

    /// <summary>
    ///
    /// </summary>
    /// <param name="aMessage"></param>
    protected void ValidationFailed(string message)
    {
      if (ValidationError != null)
      {
        ValidationError(this, message);
      }
    }

    /// <summary>
    /// Will take a snapshot of the current item
    /// </summary>
    public void Snapshot()
    {
      OriginalValue = CurrentValue;
    }

    /////////////////////////////////////////////////////

    //subscription mechanism
    public void Subscribe(Control aSubject)
    {
      Subscribe(aSubject, false, ListControlMapping.Value);
    }

    public void Subscribe(Control aSubject, ListControlMapping aListMapping)
    {
      Subscribe(aSubject, false, aListMapping);
    }

    public void Subscribe(Control aSubject, bool aAllowNulls)
    {
      Subscribe(aSubject, aAllowNulls, ListControlMapping.Value);
    }

    public void Subscribe(Control aSubject, bool aAllowNulls, ListControlMapping aListMapping)
    {
      subscription = new MappingUpdateSubscriber<T>(this, aSubject, aAllowNulls, aListMapping);
    }

    //subscription mechanism
    public void CustomizedSubscribe(MappingUpdateSubscriber<T> aSubscription)
    {
      subscription = aSubscription;
      subscription.AttachTo(this);
    }

    /// <summary>
    /// Pulls the data to "current". Used in cases where events aren't possible
    /// </summary>
    public void Pull()
    {
      Pull(false);
    }

    /// <summary>
    /// this avoids the "modified before it is used" issue when binding
    /// </summary>
    bool __BINDING__ = false;

    public void Pull(bool binding)
    {
      __BINDING__ = binding;
      try
      {
        subscription.Pull();
      }
      finally
      {
        __BINDING__ = false;
      }
    }

    /// <summary>
    /// Same as pull, except it uses "fOriginalValue"
    /// </summary>
    public void Revert()
    {
      subscription.Revert();
    }

    //////////////////////////////////////////////////////

    /// <summary>
    /// This is only really needed for lists or complex objects that don't
    /// need to match the other instances that they are related to.
    ///
    /// I'm fairly sure this is no longer needed, and so I'll revisit it later.
    /// </summary>
    /// <param name="aInstance"></param>
    /// <returns></returns>
    [Obsolete]
    protected virtual object CloneInstance(object aInstance)
    {
      Type t = aInstance.GetType();
      if (t.IsPrimitive || aInstance is String)
        return (aInstance);

      //First we create an instance of this specific type.
      object newObject = Activator.CreateInstance(t);

      //We get the array of fields for the new type instance.
      FieldInfo[] fields = newObject.GetType().GetFields();

      int i = 0;

      foreach (FieldInfo fi in aInstance.GetType().GetFields())
      {
        //We query if the fiels support the ICloneable interface.
        Type ICloneType = fi.FieldType.GetInterface("ICloneable", true);

        if (ICloneType != null)
        {
          //Getting the ICloneable interface from the object.
          ICloneable IClone = (ICloneable)fi.GetValue(aInstance);

          //We use the clone method to set the new value to the field.
          fields[i].SetValue(newObject, IClone.Clone());
        }
        else
        {
          // If the field doesn't support the ICloneable
          // interface then just set it.
          fields[i].SetValue(newObject, fi.GetValue(aInstance));
        }

        //Now we check if the object supports the
        //IEnumerable interface, so if it does
        //we need to enumerate all its items and check if
        //they support the ICloneable interface.
        Type IEnumerableType = fi.FieldType.GetInterface("IEnumerable", true);
        if (IEnumerableType != null)
        {
          //Get the IEnumerable interface from the field.
          IEnumerable IEnum = (IEnumerable)fi.GetValue(aInstance);

          //This version support the IList and the
          //IDictionary interfaces to iterate on collections.
          Type IListType = fields[i].FieldType.GetInterface("IList", true);
          Type IDicType = fields[i].FieldType.GetInterface("IDictionary", true);

          int j = 0;
          if (IListType != null)
          {
            //Getting the IList interface.
            IList list = (IList)fields[i].GetValue(newObject);

            foreach (object obj in IEnum)
            {
              //Checking to see if the current item
              //support the ICloneable interface.
              ICloneType = obj.GetType().GetInterface("ICloneable", true);

              if (ICloneType != null)
              {
                //If it does support the ICloneable interface,
                //we use it to set the clone of
                //the object in the list.
                ICloneable clone = (ICloneable)obj;
                list[j] = clone.Clone();
              }

              //NOTE: If the item in the list is not
              //support the ICloneable interface then in the
              //cloned list this item will be the same
              //item as in the original list
              //(as long as this type is a reference type).
              j++;
            }
          }
          else if (IDicType != null)
          {
            //Getting the dictionary interface.
            IDictionary dic = (IDictionary)fields[i].GetValue(newObject);
            j = 0;

            foreach (DictionaryEntry de in IEnum)
            {
              //Checking to see if the item
              //support the ICloneable interface.
              ICloneType = de.Value.GetType().GetInterface("ICloneable", true);

              if (ICloneType != null)
              {
                ICloneable clone = (ICloneable)de.Value;
                dic[de.Key] = clone.Clone();
              }
              j++;
            }
          }
        }
        i++;
      }
      return newObject;
    }

    //////////////////////////////////////////////////////

    #region Update lock

    //Oh, my.... this is SOOOO unthread safe!!

    bool __IN_UPDATE__ = false;

    /// <summary>
    /// disables updates to the underlying value
    /// </summary>
    public void BeginUpdate()
    {
      __IN_UPDATE__ = true;
    }

    /// <summary>
    ///  RE-ENABLES updates to the underlying value
    /// </summary>
    public void EndUpdate()
    {
      __IN_UPDATE__ = false;
    }

    #endregion Update lock
  }
}