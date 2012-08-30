/*
 * Copyright 2010 - 2012 Rat Cow Software and Matt Emson. All rights reserved.
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
  public class MappingUpdateSubscriber<T>
  {
    MappingObject<T> fTarget = null;
    Control fSubject = null;
    bool fAllowNulls;

    public Control Subject
    {
      get { return fSubject; }
    }

    internal MappingUpdateSubscriber(MappingObject<T> aTarget, Control aSubject)
      : this(aTarget, aSubject, false)
    {
    }

    internal MappingUpdateSubscriber(MappingObject<T> aTarget, Control aSubject, bool aAllowNulls)
    {
      fTarget = aTarget;

      //we add a fake control if aSubject is null.
      if (aSubject == null)
        fSubject = new Control();
      else
        fSubject = aSubject;

      fAllowNulls = aAllowNulls;

      HookControl();
    }

    /// <summary>
    /// Create detateched
    /// </summary>
    /// <param name="aSubject"></param>
    /// <param name="aAllowNulls"></param>
    public MappingUpdateSubscriber(Control aSubject, bool aAllowNulls)
    {
      fSubject = aSubject;
      fAllowNulls = aAllowNulls;
    }

    /// <summary>
    /// Here we get to work out how on earch we subscribe to changes...
    /// </summary>
    protected virtual void HookControl()
    {
      //listboxes and comboboxes need a special bit of co-ercing
      if (fSubject is CheckedListBox)
      {
        CheckedListBox tempCLB = (CheckedListBox)fSubject;
        tempCLB.ItemCheck += new ItemCheckEventHandler(tempCLB_ItemCheck);
      }
      else if (fSubject is ListControl)
      {
        ListControl tempLC = (ListControl)fSubject;
        tempLC.SelectedValueChanged += new EventHandler(fSubject_SelectedValueChanged);
      }
      else if (fSubject is CheckBox)
      {
        CheckBox tempCB = (CheckBox)fSubject;
        tempCB.CheckedChanged += new EventHandler(fSubject_CheckedChanged);
      }
      else if (fSubject is NumericUpDown)
      {
        NumericUpDown tempNUD = (NumericUpDown)fSubject;
        tempNUD.ValueChanged += new EventHandler(fSubject_NumericValueChanged);
      }
      else if (fSubject is DateTimePicker)
      {
        DateTimePicker tempDTP = (DateTimePicker)fSubject;
        tempDTP.ValueChanged += new EventHandler(fSubject_DateValueChanged);
      }
      else
        fSubject.TextChanged += new EventHandler(fSubject_TextChanged); //default
    }

    #region Revert the Modified flag callback

    private void CALLBACK__RevertModifiedFlag()
    {
      fTarget.fModified = false; //because we reverted... this is absolute, we have reverted to the original value
    }

    delegate void Callback();

    /// <summary>
    /// used to update lists
    /// </summary>
    internal void fTarget_ResetModifiedFlag()
    {
      Application.DoEvents();

      //only call BeginInvoke if there is a Handle, else the call fails
      if (fSubject.IsHandleCreated)
        fSubject.BeginInvoke(new Callback(CALLBACK__RevertModifiedFlag));
      else
        fTarget.fModified = false;
    }

    #endregion Revert the Modified flag callback

    /// <summary>
    /// Same as pull, except it uses "fOriginalValue"
    /// </summary>
    public virtual void Revert()
    {
      fTarget.CurrentValue = fTarget.OriginalValue;

      Pull();

      fTarget_ResetModifiedFlag();
    }

    /// <summary>
    /// Pulls the data to "current". Used in cases where events aren't possible
    /// </summary>
    public virtual void Pull()
    {
      //if we are not attached to a control (for what ever reason - usually because
      //of "fake" contollery,  we'll fail here unless we bail. If the fSubject is null
      //we assume this to be the case.
      if (fSubject == null) return;

      //listboxes and comboboxes need a special bit of co-ercing
      if (fSubject is ListControl)
      {
        ListControl tempLC = (ListControl)fSubject;
        tempLC.SelectedValue = fTarget.CurrentValue;
      }
      else if (fSubject is CheckBox)
      {
        CheckBox tempCB = (CheckBox)fSubject;
        tempCB.Checked = Convert.ToBoolean(fTarget.CurrentValue);
      }
      else if (fSubject is NumericUpDown)
      {
        NumericUpDown tempNUD = (NumericUpDown)fSubject;
        Decimal temp = Convert.ToDecimal(fTarget.CurrentValue);

        //handle overflows:
        if (temp > tempNUD.Maximum)
          tempNUD.Value = tempNUD.Maximum;
        else if (temp < tempNUD.Minimum)
          tempNUD.Value = tempNUD.Minimum;
        else
          tempNUD.Value = Convert.ToDecimal(fTarget.CurrentValue);
      }
      else if (fSubject is DateTimePicker)
      {
        DateTimePicker tempDTP = (DateTimePicker)fSubject;
        try
        {
          tempDTP.Value = Convert.ToDateTime(fTarget.CurrentValue);
        }
        catch
        {
          tempDTP.Text = String.Empty; //set null date
        }
      }
      else
        fSubject.Text = Convert.ToString(fTarget.CurrentValue); //fSubject.TextChanged += new EventHandler(fSubject_TextChanged); //default
    }

    internal void AttachTo(MappingObject<T> aTarget)
    {
      fTarget = aTarget;
      HookControl();
    }

    /// <summary>
    /// This is called to check whether the user has assigned an event handler to
    /// catch data modification.
    /// </summary>
    /// <returns></returns>
    private bool fTarget_CanModifyData(Control aSubject)
    {
      bool result = true;
      //I love how you can get round most C# limitations with a second variable of the same
      //type pointing to the same reference.
      BeforeDataModificationDelegate BeforeValueModified = fTarget.GetBeforeValueModified();
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
    private void fSubject_NumericValueChanged(object sender, EventArgs e)
    {
      if (fTarget_CanModifyData(fSubject))
      {
        fTarget.SetCurrentValueFromObject(((NumericUpDown)fSubject).Value);
      }
    }

    /// <summary>
    /// This is slightly different, we record the entire state
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected virtual void tempCLB_ItemCheck(object sender, ItemCheckEventArgs e)
    {
      //if (fTarget_CanModifyData(fSubject))
      //{
      //  fTarget.MultiItemUpdate();
      //}
    }

    /// <summary>
    /// Date time picker basaed classes..
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void fSubject_DateValueChanged(object sender, EventArgs e)
    {
      if (fTarget_CanModifyData(fSubject))
      {
        fTarget.SetCurrentValueFromObject(((DateTimePicker)fSubject).Value);
      }
    }

    /// <summary>
    /// Checkboxes
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void fSubject_CheckedChanged(object sender, EventArgs e)
    {
      if (fTarget_CanModifyData(fSubject))
      {
        fTarget.SetCurrentValueFromObject(((CheckBox)fSubject).Checked);
      }
    }

    /// <summary>
    /// This is pretty generic
    /// </summary>
    /// <param name="e"></param>
    private void fSubject_TextChanged(object sender, EventArgs e)
    {
      if (fTarget_CanModifyData(fSubject))
      {
        fTarget.SetCurrentValueFromObject(fSubject.Text);
      }
    }

    /// <summary>
    /// ListControls
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    private void fSubject_SelectedValueChanged(object sender, EventArgs e)
    {
      if (fTarget_CanModifyData(fSubject))
      {
        object value = ((ListControl)fSubject).SelectedValue;

        //                 it's null and we allow that,  or   it's not null
        bool canContinue = ((fAllowNulls & value == null) | (value != null));

        if (canContinue)
          fTarget.SetCurrentValueFromObject(value);
      }
    }
  }
}