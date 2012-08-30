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
  public interface IBaseMappingObject
  {
    void Pull();

    void Pull(bool binding);

    void Revert();

    void Subscribe(Control aSubject);

    bool Modified { get; }

    void Snapshot();

    bool IsMultiValueItem { get; set; }

    Control LinkedControl { get; set; }
  }

  public interface IMappingObject<T> : IBaseMappingObject
  {
    T CurrentValue { get; set; }

    T OriginalValue { get; }

    void Init(T aValue);

    void CustomizedSubscribe(MappingUpdateSubscriber<T> aSubscription);
  }

  public interface IMappingObject : IBaseMappingObject
  {
    object CurrentObject { get; set; }

    object OriginalObject { get; }

    void InitWithObject(object data, string propertyName);
  }

  public class MappingObject<T> : IMappingObject, IMappingObject<T>
  {
    public MappingObject()
    {
      fIsNumeric = ConversionHelper.IsNumericType(typeof(T));
    }

    public MappingObject(T aValue)
      : this()
    {
      fOriginalValue = aValue;
      CurrentValue = fOriginalValue;
      fLastUpdated = DateTime.Now;
    }

    public void Init(T aValue)
    {
      fOriginalValue = aValue;
      CurrentValue = fOriginalValue;
      fLastUpdated = DateTime.Now;
    }

    public void InitWithObject(object data, string propertyName)
    {
      dataInstance = data;
      Type dataInstanceType = dataInstance.GetType();
      dataInstanceProperty = dataInstanceType.GetProperty(propertyName);

      if (dataInstanceProperty == null) throw new NotSupportedException("The property mapping was now found");

      fOriginalValue = (T)dataInstanceProperty.GetValue(dataInstance, null); //this is now a direct load, like for like.
      fLastUpdated = DateTime.Now;
    }

    ////////////////////////////////////////////////

    public event DataModificationDelegate ValueModified = null;
    public event BeforeDataModificationDelegate BeforeValueModified = null;

    ////////////////////////////////////////////////

    #region Fields

    protected Control fLinkedControl = null;
    protected T fOriginalValue = default(T);
    //protected T fCurrentValue = default(T);
    protected DateTime fLastUpdated;

    internal bool fModified = false;
    internal bool fIsNumeric = false;

    internal object dataInstance = null;
    internal PropertyInfo dataInstanceProperty = null;

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

    /// <summary>
    /// Not to be confused with IsMultiValueItem - this represents a value where more than
    /// one item can be selected in a single field...
    /// </summary>
    protected bool fIsMultiList = false;

    /// <summary>
    /// This represents an item that points to another dataset.
    /// If this is set, we only store the fact - other code will need to load the data
    /// if we are multivalues, the value we hold is the ID of ourself, this is required to
    /// do the lookup.
    /// </summary>
    protected bool fIsMultiValueItem = false;

    /// <summary>
    /// Subscriptions go here...
    /// </summary>
    protected internal MappingUpdateSubscriber<T> fSubscription = null;

    #endregion Fields

    ////////////////////////////////////////////////

    #region Properties

    public Control LinkedControl
    {
      get { return fLinkedControl; }
      set { fLinkedControl = value; }
    }

    public T CurrentValue
    {
      //get { return fCurrentValue; }
      //set { SetCurrentValue(value); }
      get { return GetDataInstancePropertyValue(); }
      set { SetDataInstancePropertyObjectValue((object)value); }
    }

    public object CurrentObject
    {
      get { return (object)CurrentValue; }
      set { SetCurrentValueFromObject(value); }
    }

    public T OriginalValue
    {
      get { return fOriginalValue; }
    }

    public object OriginalObject
    {
      get { return (object)fOriginalValue; }
    }

    public bool Modified
    {
      get { return fModified; }
    }

    public bool IsMultiValueItem
    {
      get { return fIsMultiValueItem; }
      set { fIsMultiValueItem = value; }
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
      if (ValueModified != null)
      {
        DataModificationArgs e = new DataModificationArgs(__BINDING__);
        ValueModified(this, e);
        result = e.AllowChange;
      }

      return result;
    }

    /// <summary>
    ///
    /// </summary>
    /// <returns></returns>
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
        fLastUpdated = DateTime.Now;
        if (!__BINDING__) fModified = true;
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
        if (fIsNumeric && (newValue == null || newValue.ToString() == String.Empty))
          newValue = 0; //hmmm will this break more than it fixes?

        T newValueT = (T)Convert.ChangeType(newValue, CurrentValue.GetType());

        SetCurrentValue(newValueT);
      }
      catch (Exception ex) { ValidationFailed(ex.Message); } //we could not set the value because it was not valid in the context of the undelying generic type
    }

    /// <summary>
    /// Override this if you want something to happen
    /// </summary>
    /// <param name="aMessage"></param>
    protected virtual void ValidationFailed(string aMessage)
    {
    }

    /// <summary>
    /// Will take a snapshot of the current item
    /// </summary>
    public void Snapshot()
    {
      fOriginalValue = CurrentValue;
    }

    /////////////////////////////////////////////////////

    //subscription mechanism
    public void Subscribe(Control aSubject)
    {
      fSubscription = new MappingUpdateSubscriber<T>(this, aSubject);
    }

    //subscription mechanism
    public void CustomizedSubscribe(MappingUpdateSubscriber<T> aSubscription)
    {
      fSubscription = aSubscription;
      fSubscription.AttachTo(this);
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
        fSubscription.Pull();
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
      fSubscription.Revert();
    }

    //////////////////////////////////////////////////////

    /// <summary>
    /// This is only really needed for lists or complex objects that don't
    /// need to match the other instances that they are related to.
    /// </summary>
    /// <param name="aInstance"></param>
    /// <returns></returns>
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