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
using System.Text;
using System.Windows.Forms;

namespace RatCow.MvcFramework.Mapping
{
  using Controls;

  public enum ListControlMapping { Value, Index, Text }

  public class MappingUpdateSubscriber<T>
  {
    MappingObject<T> target = null;
    Control subject = null;
    bool allowsNulls = false;
    ListControlMapping listControlMapping; //maps int values to the index

    public Control Subject
    {
      get { return subject; }
    }

    internal MappingUpdateSubscriber(MappingObject<T> aTarget, Control aSubject)
      : this(aTarget, aSubject, false, ListControlMapping.Index)
    {
    }

    internal MappingUpdateSubscriber(MappingObject<T> aTarget, Control aSubject, bool aAllowNulls)
      : this(aTarget, aSubject, aAllowNulls, ListControlMapping.Index)
    {
    }

    internal MappingUpdateSubscriber(MappingObject<T> aTarget, Control aSubject, bool aAllowNulls, ListControlMapping aListControlMapping)
    {
      target = aTarget;

      //we add a fake control if aSubject is null.
      if (aSubject == null)
        subject = new Control(); //this was legacy.. it was used to map values we didn't need to show, but might need to manipulate
      else
        subject = aSubject;

      allowsNulls = aAllowNulls;

      listControlMapping = aListControlMapping;

      HookControl();
    }

    /// <summary>
    /// Create detateched
    /// </summary>
    /// <param name="aSubject"></param>
    /// <param name="aAllowNulls"></param>
    public MappingUpdateSubscriber(Control aSubject, bool aAllowNulls)
    {
      subject = aSubject;
      allowsNulls = aAllowNulls;
    }

    /// <summary>
    /// Here we get to work out how on earch we subscribe to changes...
    /// </summary>
    protected virtual void HookControl()
    {
      //listboxes and comboboxes need a special bit of co-ercing
      if (subject is CheckedListBox)
      {
        CheckedListBox tempCLB = (CheckedListBox)subject;
        tempCLB.ItemCheck += new ItemCheckEventHandler(tempCLB_ItemCheck);
      }
      else if (subject is ListControl)
      {
        ListControl tempLC = (ListControl)subject;
        tempLC.SelectedValueChanged += new EventHandler(subject_SelectedValueChanged);
      }
      else if (subject is CheckBox)
      {
        CheckBox tempCB = (CheckBox)subject;
        tempCB.CheckedChanged += new EventHandler(subject_CheckedChanged);
      }
      else if (subject is NumericUpDown)
      {
        NumericUpDown tempNUD = (NumericUpDown)subject;
        tempNUD.ValueChanged += new EventHandler(subject_NumericValueChanged);
      }
      else if ( subject is NullableDateTimePicker )
      {
        NullableDateTimePicker tempDTP = (NullableDateTimePicker)subject;
        tempDTP.ValueChanged += new EventHandler( subject_NullDateValueChanged );
      }
      else if (subject is DateTimePicker)
      {
        DateTimePicker tempDTP = (DateTimePicker)subject;
        tempDTP.ValueChanged += new EventHandler(subject_DateValueChanged);
      }
      else
        subject.TextChanged += new EventHandler(subject_TextChanged); //default
    }

    #region Revert the Modified flag callback

    private void CALLBACK__RevertModifiedFlag()
    {
      target.Modified = false; //because we reverted... this is absolute, we have reverted to the original value
    }

    delegate void Callback();

    /// <summary>
    /// used to update lists
    /// </summary>
    internal void target_ResetModifiedFlag()
    {
      Application.DoEvents();

      //only call BeginInvoke if there is a Handle, else the call fails
      if (subject.IsHandleCreated)
        subject.BeginInvoke(new Callback(CALLBACK__RevertModifiedFlag));
      else
        target.Modified = false;
    }

    #endregion Revert the Modified flag callback

    /// <summary>
    /// Same as pull, except it uses "fOriginalValue"
    /// </summary>
    public virtual void Revert()
    {
      target.CurrentValue = target.OriginalValue;

      Pull();

      target_ResetModifiedFlag();
    }

    /// <summary>
    /// Pulls the data to "current". Used in cases where events aren't possible
    /// </summary>
    public virtual void Pull()
    {
      //if we are not attached to a control (for what ever reason - usually because
      //of "fake" contollery,  we'll fail here unless we bail. If the subject is null
      //we assume this to be the case.
      if (subject == null) return;

      //listboxes and comboboxes need a special bit of co-ercing
      if (subject is ListControl)
      {
        ListControl tempLC = (ListControl)subject;

        switch (listControlMapping)
        {
          case ListControlMapping.Text:
            tempLC.Text = ConversionHelper.ToString( target.CurrentValue, String.Empty );
            break;

          case ListControlMapping.Index:

            tempLC.SelectedIndex = ConversionHelper.ToInt32(target.CurrentValue, -1);
            break;

          case ListControlMapping.Value:
          default:
            tempLC.SelectedValue = target.CurrentValue;
            break;
        }
      }
      else if (subject is CheckBox)
      {
        CheckBox tempCB = (CheckBox)subject;
        tempCB.Checked = Convert.ToBoolean(target.CurrentValue);
      }
      else if (subject is NumericUpDown)
      {
        NumericUpDown tempNUD = (NumericUpDown)subject;
        Decimal temp = Convert.ToDecimal(target.CurrentValue);

        //handle overflows:
        if (temp > tempNUD.Maximum)
          tempNUD.Value = tempNUD.Maximum;
        else if (temp < tempNUD.Minimum)
          tempNUD.Value = tempNUD.Minimum;
        else
          tempNUD.Value = Convert.ToDecimal(target.CurrentValue);
      }
      else if ( subject is NullableDateTimePicker )
      {
        NullableDateTimePicker tempDTP = (NullableDateTimePicker)subject;
        try
        {
          tempDTP.Value = Convert.ToDateTime( target.CurrentValue );
        }
        catch
        {
          tempDTP.Value = tempDTP.NullDate;
        }
      }
      else if (subject is DateTimePicker)
      {
        DateTimePicker tempDTP = (DateTimePicker)subject;
        try
        {
          tempDTP.Value = Convert.ToDateTime(target.CurrentValue);
        }
        catch
        {
          tempDTP.Text = String.Empty; //set null date
        }
      }
      else
        subject.Text = Convert.ToString(target.CurrentValue); //subject.TextChanged += new EventHandler(subject_TextChanged); //default
    }

    internal void AttachTo(MappingObject<T> aTarget)
    {
      target = aTarget;
      HookControl();
    }

    /// <summary>
    /// This is called to check whether the user has assigned an event handler to
    /// catch data modification.
    /// </summary>
    /// <returns></returns>
    private bool target_CanModifyData(Control aSubject)
    {
      bool result = true;
      //I love how you can get round most C# limitations with a second variable of the same
      //type pointing to the same reference.
      BeforeDataModificationDelegate BeforeValueModified = target.GetBeforeValueModified();
      if (BeforeValueModified != null)
      {
        BeforeDataModification e = new BeforeDataModification(aSubject);
        BeforeValueModified(this, e);
        result = e.AllowChange;
      }

      if (!result) Revert();  //go back to what we had before (this will probably screw up the modified state..)

      return result;
    }

    /// <summary>
    /// Can this be merged with the DateTimePicker?
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void subject_NumericValueChanged(object sender, EventArgs e)
    {
      if (target_CanModifyData(subject))
      {
        target.SetCurrentValueFromObject(((NumericUpDown)subject).Value);
      }
    }

    /// <summary>
    /// This is slightly different, we record the entire state
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void tempCLB_ItemCheck(object sender, ItemCheckEventArgs e)
    {
      //if (target_CanModifyData(subject))
      //{
      //  target.MultiItemUpdate();
      //}
    }

    /// <summary>
    /// Date time picker basaed classes..
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void subject_DateValueChanged(object sender, EventArgs e)
    {
      if (target_CanModifyData(subject))
      {
        target.SetCurrentValueFromObject(((DateTimePicker)subject).Value);
      }
    }

    private void subject_NullDateValueChanged( object sender, EventArgs e )
    {
      if ( target_CanModifyData( subject ) )
      {
        target.SetCurrentValueFromObject( ( (NullableDateTimePicker)subject ).Value );
      }
    }

    /// <summary>
    /// Checkboxes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void subject_CheckedChanged(object sender, EventArgs e)
    {
      if (target_CanModifyData(subject))
      {
        target.SetCurrentValueFromObject(((CheckBox)subject).Checked);
      }
    }

    /// <summary>
    /// This is pretty generic
    /// </summary>
    /// <param name="e"></param>
    private void subject_TextChanged(object sender, EventArgs e)
    {
      if (target_CanModifyData(subject))
      {
        target.SetCurrentValueFromObject(subject.Text);
      }
    }

    /// <summary>
    /// ListControls
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void subject_SelectedValueChanged(object sender, EventArgs e)
    {
      if (target_CanModifyData(subject))
      {
        object value = null;
        switch (listControlMapping)
        {
          case ListControlMapping.Text:
            value = ((ListControl)subject).Text;

            //we assume this will be a non zero number or -1
            target.SetCurrentValueFromObject(value);

            break;

          case ListControlMapping.Index:

            value = ((ListControl)subject).SelectedIndex;

            //we assume this will be a non zero number or -1
            target.SetCurrentValueFromObject(value);
            break;

          case ListControlMapping.Value: //this is the legacy default
          default:
            value = ((ListControl)subject).SelectedValue;

            //                 it's null and we allow that,  or   it's not null
            bool canContinue = ((allowsNulls & value == null) | (value != null));

            if (canContinue)
              target.SetCurrentValueFromObject(value);
            break;
        }
      }
    }
  }
}